﻿using System.Text.Json.Serialization;

using Avalonia.Collections;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SourceGit.ViewModels
{
    public class RepositoryNode : ObservableObject
    {
        public string Id
        {
            get => _id;
            set
            {
                var normalized = value.Replace('\\', '/');
                SetProperty(ref _id, normalized);
            }
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public int Bookmark
        {
            get => _bookmark;
            set => SetProperty(ref _bookmark, value);
        }

        public bool IsRepository
        {
            get => _isRepository;
            set => SetProperty(ref _isRepository, value);
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }

        [JsonIgnore]
        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        public AvaloniaList<RepositoryNode> SubNodes
        {
            get => _subNodes;
            set => SetProperty(ref _subNodes, value);
        }

        public void Edit()
        {
            if (PopupHost.CanCreatePopup())
                PopupHost.ShowPopup(new EditRepositoryNode(this));
        }

        public void AddSubFolder()
        {
            if (PopupHost.CanCreatePopup())
                PopupHost.ShowPopup(new CreateGroup(this));
        }

        public void OpenInFileManager()
        {
            if (!IsRepository)
                return;
            Native.OS.OpenInFileManager(_id);
        }

        public void OpenTerminal()
        {
            if (!IsRepository)
                return;
            string tabColor = null;
            if (Bookmark >= 0)
            {
                
                if (Models.Bookmarks.Brushes[Bookmark] is ISolidColorBrush brush)
                {
                    tabColor = $"#{brush.Color.R:X2}{brush.Color.G:X2}{brush.Color.B:X2}";
                }
            }
            Native.OS.OpenTerminal(_id, tabColor);
        }

        public void Delete()
        {
            if (PopupHost.CanCreatePopup())
                PopupHost.ShowPopup(new DeleteRepositoryNode(this));
        }

        private string _id = string.Empty;
        private string _name = string.Empty;
        private bool _isRepository = false;
        private int _bookmark = 0;
        private bool _isExpanded = false;
        private bool _isVisible = true;
        private AvaloniaList<RepositoryNode> _subNodes = [];
    }
}
