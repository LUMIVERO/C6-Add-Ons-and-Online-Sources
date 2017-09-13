using System;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;

using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Collections;
using SwissAcademic.Citavi.Persistence;

// Implementation of macro editor is preliminary and experimental.
// The Citavi object model is subject to change in future version.

public static class CitaviMacro
{
	public static void Main()
	{
		ProjectShell activeProjectShell = Program.ActiveProjectShell;
		if (activeProjectShell == null) return; //no open project shell
		
		Project activeProject = Program.ActiveProjectShell.Project;
		if (activeProject == null) return;
		
		Form primaryMainForm = activeProjectShell.PrimaryMainForm;
		if (primaryMainForm == null) return;
		
		string xmlFile = string.Empty;
		string ctv5File = string.Empty;
		string initialDirectory = Program.Engine.DesktopEngineConfiguration.GetFolderPath(CitaviFolder.Projects, activeProject);

		//(RE)IMPORT
		using (OpenFileDialog openFileDialog = new OpenFileDialog())
		{
			openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
			openFileDialog.Title = "Select an XML file with Citavi project data for import";	
				
			if (openFileDialog.ShowDialog() != DialogResult.OK) return;
			xmlFile = openFileDialog.FileName;
			if (string.IsNullOrEmpty(xmlFile)) return;
			if (!File.Exists(xmlFile)) return;
		}

		bool goOn = true;
		while (goOn)
		{
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				saveFileDialog.Filter =  "Citavi 5 files (*.ctv5)|*.ctv5";
				saveFileDialog.InitialDirectory = initialDirectory;
				saveFileDialog.Title = "Enter a C5 file name to create a new C5 project with the imported data";

				if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
				ctv5File = saveFileDialog.FileName;
			}
				
			if (string.IsNullOrEmpty(ctv5File)) return;
				
			if (File.Exists(ctv5File))
			{
				if (MessageBox.Show(string.Format("Do you want to overwrite the file {0}?", ctv5File), "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) goOn = false;			
			}
			else
			{
				goOn = false;
			}
		}
		var config = new DesktopProjectConfiguration(Program.Engine,ctv5File);
		var project = Program.Engine.Projects.Open(config);
		XmlToProject.Load(xmlFile, project);
		
		MessageBox.Show("Finished");
	}
}