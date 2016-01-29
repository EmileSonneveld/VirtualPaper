#if UNITY_EDITOR && UNITY_5
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;


[InitializeOnLoad]
public class AkWwiseFirstImportCallback
{
    const int INTEGRATION_NUMBER_2013_2_8 = 1;
    const int INTEGRATION_NUMBER_2014_1_BETA = 2;
    const int BUILD_NUMBER_2013_2_8 = 4865;
    const int BUILD_NUMBER_2014_1_BETA = 5073;

    static readonly string POST_IMPORT_CALLBACK_PATH = Application.dataPath + "/Wwise/Editor/WwiseSetupWizard/AkWwisePostImportCallback.cs";
    private static bool m_SameVersion = false;


	[DidReloadScripts(1000000)]
    static void PostImportFunction()
    {
        try
        {
            // Do nothing in batch mode
            // This will prevent the script from running when building the unity package
            string[] arguments = Environment.GetCommandLineArgs();
            if (Array.IndexOf(arguments, "-nographics") != -1)
            {
                return;
            }

			if( File.Exists(POST_IMPORT_CALLBACK_PATH) )
			{
				Debug.Log("WwiseUnity: Found old post import callback, delete it" + POST_IMPORT_CALLBACK_PATH);
				AssetDatabase.DeleteAsset("Assets/Wwise/Editor/WwiseSetupWizard/AkWwisePostImportCallback.cs");
			}
			
			// Cleanup duplicated files created by the package import. This is because there is a GUID mismatch between old and new packages.
			RemoveDuplicateFiles();
			
			RemoveOldMetro();

            //We check if an integration is already installed or if this is a first installation
            if (File.Exists(Application.dataPath + "/Wwise/Version.txt"))
            {
                //We get the Build number of the integration that is currently installed
				int buildNumber = -1;
                int integrationNumber = GetIntegrationNumber(Application.dataPath + "/Wwise/Version.txt", out buildNumber);
				
				//If the installed integration is older than 2013.2.8
				if( integrationNumber == -1 )
				{
					VeryOldVersion();
				}
				else
				{
					//We get the Build number of the integration that will be installed

					int newBuildNumber = -1;
					int newIntegrationNumber = GetIntegrationNumber(Application.dataPath + "/Wwise/NewVersion.txt", out newBuildNumber);
					
					//If the build numbers don't match, we need a migration
					if (integrationNumber < newIntegrationNumber && integrationNumber > 0)
					{
						Debug.Log("WwiseUnity: Preparing migration...");
						Migrate();
					}
					//Same build number so no migration is needed
					else
					{
						if (buildNumber == newBuildNumber)
						{
							SameWwiseVersion();
						}
						else
						{
							DifferentWwiseVersion();
						}

						SameUnityVersion();
						m_SameVersion = true;
					}

				}
            }
			else if( Directory.Exists(Application.dataPath + Path.DirectorySeparatorChar + "Wwise" + Path.DirectorySeparatorChar + "Deployment" + Path.DirectorySeparatorChar + "Examples") )
			{
				VeryOldVersion();
			}
            else
            {
                FirstInstallation();
            }

            //Refresh the assets database to trigger a recompilation
            if (m_SameVersion)
            {
                Debug.Log("WwiseUnity: New Wwise Unity Integration package imported successfully.");
            }
            else
            {
                Debug.Log("WwiseUnity: Compile scripts...");
            }

			AssetDatabase.DeleteAsset("Assets/Wwise/Editor/WwiseSetupWizard/AkWwiseFirstImportCallback.cs");
			AssetDatabase.Refresh();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

	static void RemoveDuplicateFiles()
	{
		try
		{
			DirectoryInfo di = new DirectoryInfo(Path.Combine(Application.dataPath, "Wwise"));
			FileInfo[] filesToDelete = di.GetFiles ("* 1.*", SearchOption.AllDirectories);
			foreach(FileInfo file in filesToDelete)
			{
				if( file.Extension == ".meta" )
				{
					continue;
				}
				
				// We need to keep the newest file (the one with the "1" in it)
				string FilePath = file.FullName;
				FilePath = FilePath.Remove (0, Application.dataPath.Length+1); // Remove slash between "Assets" and "Wwise"
				FilePath = Path.Combine ("Assets", FilePath);
				int index = FilePath.IndexOf(file.Extension);
				string renameTo = FilePath.Remove(index - 2, file.Extension.Length + 2);
				string oldFilePath = renameTo + file.Extension;
				renameTo = renameTo.Remove (0, renameTo.LastIndexOf(Path.DirectorySeparatorChar)+1);
				
				AssetDatabase.DeleteAsset(oldFilePath);
				AssetDatabase.RenameAsset(FilePath, renameTo);
			}
			
			// Special case for the one file we have in StreamingAssets
			if( File.Exists( Path.Combine(Path.Combine(Application.dataPath, "StreamingAssets"), "desc 1.txt")) )
			{
				File.Delete(Path.Combine(Path.Combine(Application.dataPath, "StreamingAssets"), "desc 1.txt"));
			}
			
			AssetDatabase.Refresh ();
		}
		catch (Exception)
		{
			// We don't want this to print errors. If it fails, it will not prevent the integration from working
		}
	}
	
	static void RemoveOldMetro()
	{
		if(Directory.Exists(Application.dataPath + "/Wwise/Deployment/Plugins/WSA_New"))
		{
			string[] MetroPaths = {
				Application.dataPath + "/Wwise/Deployment/API/Generated/Metro",
				Application.dataPath + "/Wwise/Deployment/Plugins/Metro",
				Application.dataPath + "/Wwise/Editor/WwiseMenu/Metro"
			};
				
			foreach(string PathToDelete in MetroPaths)
			{
				if( Directory.Exists(PathToDelete) )
				{
					Directory.Delete(PathToDelete, true);
				}
			}
		}
	}

	static void VeryOldVersion()
	{
		DeleteScripts(new DirectoryInfo(Application.dataPath + "/Wwise/Deployment/API"), "*.cs");

		DirectoryInfo[] platformPluginFolders = new DirectoryInfo(Application.dataPath + "/Wwise/Deployment/Plugins").GetDirectories();
		foreach (DirectoryInfo folder in platformPluginFolders)
		{
			if (!folder.Name.Contains("_new"))
				folder.Delete(true);
		}

		FirstInstallation();
	}
	

    static void Migrate()
    {
        try
        {
            Debug.Log("WwiseUnity: Import migration scripts...");

            //Rename scripts needed for the migration so they can be recompiled by unity
            RenameFile(Application.dataPath + "/Wwise/Editor/WwiseSetupWizard/AkPluginActivator.new", Application.dataPath + "/Wwise/Editor/WwiseSetupWizard/AkPluginActivator.cs");
			
            Debug.Log("WwiseUnity: Import: " + POST_IMPORT_CALLBACK_PATH);
            RenameFile(Application.dataPath + "/Wwise/Editor/WwiseSetupWizard/AkWwisePostImportCallback.migration_setup", POST_IMPORT_CALLBACK_PATH);

            string migrationWindowPath = Application.dataPath + "/Wwise/Editor/WwiseSetupWizard/AkWwiseMigrationWindow.cs";
            Debug.Log("WwiseUnity: Import: " + migrationWindowPath);
            RenameFile(Application.dataPath + "/Wwise/Editor/WwiseSetupWizard/AkWwiseMigrationWindow.migration_setup", migrationWindowPath);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    static void SameWwiseVersion()
    {
        try
        {
            Debug.Log("WwiseUnity: Integration already installed.");
            Debug.Log("WwiseUnity: Deleting all imported files...");

            //Check if a new platform was added in the Wwise/Deployment/API/Generated folder. 
            CheckForNewPlatforms("Wwise/Deployment/API/Generated");

            //Check if a new platform was added in the /Wwise/Editor/WwiseMenu folder. 
            CheckForNewPlatforms("Wwise/Editor/WwiseMenu");

            DirectoryInfo[] platformPluginFolders = new DirectoryInfo(Application.dataPath + "/Wwise/Deployment/Plugins").GetDirectories();
            foreach (DirectoryInfo folder in platformPluginFolders)
            {
                //If the platform folder was just imported...
                if (folder.Name.Contains("_new"))
                {
                    //if the platform folder was already there...
                    if (Directory.Exists(folder.FullName.Remove(folder.FullName.Length - 4)))
                    {
                        //we delete the newly imported platform folder
                        AssetDatabase.DeleteAsset("Assets" + folder.FullName.Substring(Application.dataPath.Length));
                    }
                    //if the platform folder wasn't there...
                    else
                    {
                        //Rename the platform folder by removing "_new"
                        Directory.Move(folder.FullName, folder.FullName.Remove(folder.FullName.Length - 4));
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    static void DifferentWwiseVersion()
    {
        try
        {
            Debug.Log("WwiseUnity: Installing new Wwise API and plugin...");

            //Check if a new platform was added in the Wwise/Deployment/API/Generated folder. 
            OverwriteAPI();

            // Delete the old plugins, and keep the new
            DirectoryInfo[] platformPluginFolders = new DirectoryInfo(Application.dataPath + "/Wwise/Deployment/Plugins").GetDirectories();
            foreach (DirectoryInfo folder in platformPluginFolders)
            {
                //If the platform folder was just imported...
                if (folder.Name.Contains("_new"))
                {
                    //if the platform folder was already there...
                    if (Directory.Exists(folder.FullName.Remove(folder.FullName.Length - 4)))
                    {
                        Directory.Delete(folder.FullName.Remove(folder.FullName.Length - 4), true);
                    }

                    //Rename the platform folder by removing "_new"
                    Directory.Move(folder.FullName, folder.FullName.Remove(folder.FullName.Length - 4));
                }
            }
			
            // Remove the plugins in the plugins folder
            string pluginsDir = Path.Combine(Application.dataPath, "Plugins");
            if( Directory.Exists(pluginsDir) )
            {
	            string[] foundPlugins = Directory.GetFiles(pluginsDir, "AkSoundEngine*", SearchOption.AllDirectories);
	            foreach (string plugin in foundPlugins)
	            {
	                File.Delete(plugin);
	            }
	        }
			
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    static void SameUnityVersion()
    {
        try
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + "/Wwise");

            //Activate all imported editor scripts with the .new extension
            ActivateNewScripts(dirInfo);

            //Delete the intermediate migration files
            DeleteScripts(dirInfo, "*.migration_*");

            //Delete the migration setup files
            File.Delete(Application.dataPath + "/Wwise/Editor/WwiseSetupWizard/AkWwisePostImportCallback.migration_setup");
            File.Delete(Application.dataPath + "/Wwise/Editor/WwiseSetupWizard/AkWwiseMigrationWindow.migration_setup");

            //Delete NewVersion.txt
            File.Delete(Application.dataPath + "/Wwise/NewVersion.txt");

            //Rename the setup file so it can be compiled by Unity
            Debug.Log("WwiseUnity: Import installation script...");
            Debug.Log("WwiseUnity: Import: " + Application.dataPath + "/Wwise/Editor/WwiseSetupWizard/AkWwisePostImportCallback.cs");

            RenameFile(Application.dataPath + "/Wwise/Editor/WwiseSetupWizard/AkWwisePostImportCallback.setup", POST_IMPORT_CALLBACK_PATH);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    static void FirstInstallation()
    {
        try
        {
            Debug.Log("WwiseUnity: Preparing first installation...");

            DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + "/Wwise");

            //Changing the extension from .new to .cs so they can get compiled by unity 
            Debug.Log("WwiseUnity: Importing new scripts...");
            FileInfo[] files = dirInfo.GetFiles("*.new", SearchOption.AllDirectories);
            foreach (FileInfo file in files)
            {
                RenameFile(file.FullName, Path.ChangeExtension(file.FullName, ".cs"));
            }

            //Remove the "_new" in the platform plugin folder name
            DirectoryInfo[] platformPluginFolders = new DirectoryInfo(Application.dataPath + "/Wwise/Deployment/Plugins").GetDirectories();
            foreach (DirectoryInfo folder in platformPluginFolders)
            {
                Directory.Move(folder.FullName, folder.FullName.Remove(folder.FullName.Length - 4));
            }

            //Delete the intermediate migration files
            Debug.Log("WwiseUnity: Deleting migration files...");
            DeleteScripts(dirInfo, "*.migration_*");

            //Delete the migration setup files
            Debug.Log("WwiseUnity: Deleting migration setup files...");
            Debug.Log(Application.dataPath + "/Wwise/Editor/WwiseSetupWizard/AkWwisePostImportCallback.migration_setup");
            AssetDatabase.DeleteAsset("Assets/Wwise/Editor/WwiseSetupWizard/AkWwisePostImportCallback.migration_setup");
            Debug.Log(Application.dataPath + "/Wwise/Editor/WwiseSetupWizard/AkWwiseMigrationWindow.migration_setup");
            AssetDatabase.DeleteAsset("Assets/Wwise/Editor/WwiseSetupWizard/AkWwiseMigrationWindow.migration_setup");

            //Rename the version file to Version.txt
            RenameFile(Application.dataPath + "/Wwise/NewVersion.txt", Application.dataPath + "/Wwise/Version.txt");

            //Rename the setup file so it can be compiled by Unity
            Debug.Log("WwiseUnity: Import installation script...");
            Debug.Log("WwiseUnity: Import: " + POST_IMPORT_CALLBACK_PATH);
            RenameFile(Application.dataPath + "/Wwise/Editor/WwiseSetupWizard/AkWwisePostImportCallback.setup", POST_IMPORT_CALLBACK_PATH);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    static void DeleteScripts(DirectoryInfo in_dir, string in_searchPattern)
    {
        FileInfo[] files = in_dir.GetFiles(in_searchPattern, SearchOption.AllDirectories);
        foreach (FileInfo file in files)
        {
            file.Delete();
        }
    }

    static void ActivateNewScripts(DirectoryInfo in_dir)
    {
        FileInfo[] files = in_dir.GetFiles("*.new", SearchOption.AllDirectories);
        foreach (FileInfo file in files)
        {
			RenameFile(file.FullName, Path.ChangeExtension(file.FullName, ".cs"));
        }
    }

    static void CheckForNewPlatforms(string in_path)
    {
        //Check if a new platform was added in 'in_path' folder. 
        DirectoryInfo[] platformApiFolders = new DirectoryInfo(Application.dataPath + "/" + in_path).GetDirectories();
        foreach (DirectoryInfo folder in platformApiFolders)
        {
            if (folder.GetFiles("*.cs", SearchOption.AllDirectories).Length == 0)
            {
                //A new platform was added so we change the script's extension so they can be compiled
                FileInfo[] files = folder.GetFiles("*.new", SearchOption.AllDirectories);
                foreach (FileInfo file in files)
                {
                    RenameFile(file.FullName, Path.ChangeExtension(file.FullName, ".cs"));
                }
            }
        }
    }

    static void OverwriteAPI()
    {
        //Check if a new platform was added in 'in_path' folder. 
        List<DirectoryInfo> ApiFolders = new List<DirectoryInfo>();
        ApiFolders.AddRange(new DirectoryInfo(Application.dataPath + "/Wwise/Deployment/API/Generated").GetDirectories());
        ApiFolders.Add(new DirectoryInfo(Application.dataPath + "/Wwise/Deployment/API/Handwritten"));
        foreach (DirectoryInfo folder in ApiFolders)
        {
            if (folder.GetFiles("*.new", SearchOption.AllDirectories).Length != 0)
            {
                // Delete the old API
                FileInfo[] oldFiles = folder.GetFiles("*.cs", SearchOption.AllDirectories);
                foreach (FileInfo file in oldFiles)
                {
                    file.Delete();
                }

                // Install the new API
                FileInfo[] newFiles = folder.GetFiles("*.new", SearchOption.AllDirectories);
                foreach (FileInfo file in newFiles)
                {
                    RenameFile(file.FullName, Path.ChangeExtension(file.FullName, ".cs"));
                }
            }
        }
    }

    static void RenameFile(string in_path, string in_newPath)
    {
        File.Delete(in_newPath);
        File.Move(in_path, in_newPath);
        File.SetLastWriteTime(in_newPath, DateTime.Now);
    }

	static int GetIntegrationNumber(string filePath, out int out_buildNumber)
	{
		string[] VersionTxtLines;
		int integrationNumber = -1;
		out_buildNumber = -1;
		try
		{
			VersionTxtLines = System.IO.File.ReadAllLines(filePath);
			foreach(string line in VersionTxtLines)
			{
				if( line.StartsWith("Based on Wwise SDK") )
				{
					int IndexOfToken = line.IndexOf("Build ");
					int NumberStartIndex = IndexOfToken + "Build ".Length;
					int SubStrLength = line.Length - NumberStartIndex;
					string integrationBuildNumberString = line.Substring(NumberStartIndex, SubStrLength);
					out_buildNumber = Convert.ToInt32(integrationBuildNumberString);				
				}
				else if( line.StartsWith ("Unity Integration Version:") )
				{
					int NumberStartIndex = "Unity Integration Version: ".Length;
					int SubStrLength = line.Length - NumberStartIndex;
					string integrationNumberString = line.Substring(NumberStartIndex, SubStrLength);
					integrationNumber = Convert.ToInt32(integrationNumberString);
				}
			}
		}
		catch(Exception e)
		{
			Debug.LogError ("WwiseUnity: Could not get installed Wwise version. " + e.ToString ());
			return -1;
		}
		
		// Special cases for older integrations that did not have a version number
		if( integrationNumber == -1 )
		{
			if (out_buildNumber == BUILD_NUMBER_2014_1_BETA)
			{
				integrationNumber = INTEGRATION_NUMBER_2014_1_BETA;
			}
			else if (out_buildNumber == BUILD_NUMBER_2013_2_8)
			{
				integrationNumber = INTEGRATION_NUMBER_2013_2_8;
			}
		}

		return integrationNumber;
	}

}

#endif // UNITY_EDITOR