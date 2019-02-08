#region License

// ====================================================
// NautilusFT Project by shaliuno.
// This program comes with ABSOLUTELY NO WARRANTY; This is free software,
// and you are welcome to redistribute it under certain conditions; See
// file LICENSE, which is part of this source code package, for details.
// ====================================================

#endregion

using BrightIdeasSoftware;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace NautilusFT
{
    public partial class FormMain : Form
    {
        // Text search filters
        private TextMatchFilter filter;
        private HighlightTextRenderer hfilter;

        // Header Format
        private HeaderFormatStyle headerStyle = new HeaderFormatStyle();
        private HeaderFormatStyle headerStyleEmoji = new HeaderFormatStyle();

        // Fonts
        private Font fontBold = new Font(DefaultFont, FontStyle.Bold);
        private Font fontEmoji = new Font("Segoe UI Emoji", 8.25f, FontStyle.Regular);
        private Font fontUnderline = new Font(DefaultFont, FontStyle.Bold | FontStyle.Underline);

        private bool findLootListClear = false;
        private bool findLootListClearLootItems = false;

        private void InitializeListView()
        {
            headerStyle.SetFont(fontBold);
            headerStyle.Hot.Font = fontUnderline;

            headerStyleEmoji.SetFont(fontEmoji);
            olvLootList.EmptyListMsg = "Nothing for now";

            // This is changed in form but I keep track of what has changed from default.
            olvLootList.GridLines = true;
            olvLootList.ShowGroups = false;
            olvLootList.FullRowSelect = true;
            olvLootList.UseHyperlinks = false;

            // No context menu filter on column right click.
            olvLootList.SelectColumnsOnRightClick = false;
            olvLootList.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.None;

            olvLootList.TintSortColumn = false;

            // Textbox search filter.
            olvLootList.UseFiltering = true;

            // Cell format events, so we can apply different colors and wonts
            olvLootList.UseCellFormatEvents = true;

            olvLootList.ShowSortIndicators = true;

            olvLootList.MultiSelect = true;
            olvLootList.UseCustomSelectionColors = true;

            olvLootList.HasCollapsibleGroups = true;

            olvLootList.Font = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold);

            // Declare what columns are searchable
            columnName.Searchable = true;
            columnEnabled.Searchable = true;

            // Get Lootables list.
            olvLootList.SetObjects(LootListView.GetLootLists());

            // Use always on every change like drawing stuff etc.
            Olv_ParseEntries();
            olvLootList.Sort(olvLootList.GetColumn(0), SortOrder.Ascending);
        }

        private void Olv_LootList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (olvLootList.SelectedItems.Count > 1 && !findLootListClear && Settings.LootListForListView.Count > 0)
            {
                return;
            }

            var selectedIndex = olvLootList.SelectedIndex;
            var idx = Settings.LootListForListView.IndexOf(new LootListView(olvLootList.GetSubItem(selectedIndex, 0).Text, string.Empty), 0, Settings.LootListForListView.Count);

            if (Settings.LootListForListView[idx].Status == "-" && olvLootList.GetSubItem(selectedIndex, columnEnabled.DisplayIndex).Text == "-")
            {
                Settings.LootListForListView[idx].Status = "+";
                olvLootList.GetSubItem(selectedIndex, columnEnabled.DisplayIndex).Text = "+";
                for (int i = 0; i < olvLootList.Columns.Count; i++)
                {
                    olvLootList.GetSubItem(selectedIndex, i).ForeColor = Color.Green;
                }

                var selectedName = olvLootList.GetSubItem(selectedIndex, 0).Text;

                if (!Settings.LootListToLookFor.Contains(selectedName.ToString()))
                {
                    Settings.LootListToLookFor.Add(selectedName.ToString());
                }
            }
            else
            {
                Settings.LootListForListView[idx].Status = "-";
                olvLootList.GetSubItem(selectedIndex, columnEnabled.DisplayIndex).Text = "-";
                for (int i = 0; i < olvLootList.Columns.Count; i++)
                {
                    olvLootList.GetSubItem(selectedIndex, i).ForeColor = Color.Red;
                }

                var selectedName = olvLootList.GetSubItem(selectedIndex, 0).Text;

                while (Settings.LootListToLookFor.Contains(selectedName.ToString()))
                {
                    Settings.LootListToLookFor.Remove(selectedName.ToString());
                }
            }
        }

        private void Olv_ParseEntries()
        {
            var a = olvLootList.GetItemCount();

            for (int j = 0; j < a; j++)
            {
                if (olvLootList.GetSubItem(j, columnEnabled.DisplayIndex).Text == "-")
                {
                    if (Settings.LootListToLookFor.Contains(olvLootList.GetSubItem(j, 1).Text))
                    {
                        olvLootList.GetSubItem(j, columnEnabled.DisplayIndex).Text = "+";
                        for (int i = 0; i <= 5; i++)
                        {
                            olvLootList.GetSubItem(j, i).ForeColor = Color.Green;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < olvLootList.Columns.Count; i++)
                        {
                            olvLootList.GetSubItem(j, i).ForeColor = Color.Red;
                        }
                    }
                }
                else if (olvLootList.GetSubItem(j, columnEnabled.DisplayIndex).Text == "+")
                {
                    for (int i = 0; i < olvLootList.Columns.Count; i++)
                    {
                        olvLootList.GetSubItem(j, i).ForeColor = Color.Green;
                    }
                }
            }
        }

        private void Olv_ShowGroupsCheck_CheckedChanged(object sender, EventArgs e)
        {
            olvLootList.ShowGroups = olvShowGroupsCheck.Checked;
            olvLootList.BuildList(true);
            Olv_ParseEntries();
        }

        private void TextBox_Search_TextChanged(object sender, EventArgs e)
        {
            filter = TextMatchFilter.Contains(olvLootList, textBoxSearch.Text);
            hfilter = new HighlightTextRenderer(filter);
            olvLootList.ModelFilter = filter;
            olvLootList.DefaultRenderer = hfilter;
            hfilter.UseRoundedRectangle = false;
            Olv_ParseEntries();
        }

        private void Button_Clear_Click(object sender, EventArgs e)
        {
            findLootListClear = true;
        }

        private void Button_ClearAll_Click(object sender, EventArgs e)
        {
            findLootListClear = true;
            findLootListClearLootItems = true;
        }
    }
}