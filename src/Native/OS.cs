﻿using System;
using System.Collections.Generic;
using System.IO;

using Avalonia;

namespace SourceGit.Native
{
    public static class OS
    {
        public interface IBackend
        {
            void SetupApp(AppBuilder builder);

            string FindGitExecutable();
            List<Models.ExternalTool> FindExternalTools();

            void OpenTerminal(string workdir, string tabColor);
            void OpenInFileManager(string path, bool select);
            void OpenBrowser(string url);
            void OpenWithDefaultEditor(string file);
        }

        public static readonly string DataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SourceGit");
        public static string GitExecutable { get; set; } = string.Empty;
        public static List<Models.ExternalTool> ExternalTools { get; set; } = new List<Models.ExternalTool>();

        static OS()
        {
            if (OperatingSystem.IsWindows())
            {
                _backend = new Windows();
            }
            else if (OperatingSystem.IsMacOS())
            {
                _backend = new MacOS();
            }
            else if (OperatingSystem.IsLinux())
            {
                _backend = new Linux();
            }
            else
            {
                throw new Exception("Platform unsupported!!!");
            }
        }

        public static Models.Shell GetShell()
        {
            if (OperatingSystem.IsWindows())
            {
                return (_backend as Windows).Shell;
            }
            else
            {
                return Models.Shell.Default;
            }
        }

        public static bool SetShell(Models.Shell shell, bool opentInNewTab)
        {
            if (OperatingSystem.IsWindows())
            {
                var windows = _backend as Windows;
                if (windows.Shell != shell)
                {
                    windows.Shell = shell;
                    return true;
                }
                if (windows.OpenInNewTab != opentInNewTab)
                {
                    windows.OpenInNewTab = opentInNewTab;
                    return true;
                }
            }

            return false;
        }

        public static void SetupApp(AppBuilder builder)
        {
            if (!Directory.Exists(DataDir))
                Directory.CreateDirectory(DataDir);

            _backend.SetupApp(builder);
        }

        public static void SetupEnternalTools()
        {
            ExternalTools = _backend.FindExternalTools();
        }

        public static string FindGitExecutable()
        {
            return _backend.FindGitExecutable();
        }

        public static void OpenInFileManager(string path, bool select = false)
        {
            _backend.OpenInFileManager(path, select);
        }

        public static void OpenBrowser(string url)
        {
            _backend.OpenBrowser(url);
        }

        public static void OpenTerminal(string workdir, string tabColor = default)
        {
            _backend.OpenTerminal(workdir, tabColor);
        }

        public static void OpenWithDefaultEditor(string file)
        {
            _backend.OpenWithDefaultEditor(file);
        }

        private static IBackend _backend = null;
    }
}
