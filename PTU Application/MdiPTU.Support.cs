#region --- Revision History ---
/*
 * 
 *  This document and its contents are the property of Bombardier Inc. or its subsidiaries and contains confidential, proprietary information.
 *  The reproduction, distribution, utilization or the communication of this document, or any part thereof, without express authorization is strictly prohibited.  
 *  Offenders will be held liable for the payment of damages.
 * 
 *  (C) 2010    Bombardier Inc. or its subsidiaries. All rights reserved.
 * 
 *  Solution:   Portable Test Unit
 * 
 *  Project:    PTU Application
 * 
 *  File name:  MdiPTU.Support.cs
 * 
 *  Revision History
 *  ----------------
 */

#region - [1.0 to 1.23] -
/* 
 *  Date        Version Author          Comments
 *  04/06/10    1.0     K.McD           1.  First entry into TortoiseSVN.
 * 
 *  08/16/10    1.1     K.McD           1.  Bug fix SNCR 001.009. If the user exits the replay screen directly rather than returning back via the YT plot screen, 
 *                                          the function keys are not restored correctly. Added the ToolStripItemCollectionMainWindow property.
 * 
 *  08/18/10    1.2     K.McDonald      1.  Added reference to the self-test menu interface.
 * 
 *  08/24/10    1.3     K.McD           1.  Moved the menu handling methods: SetMenuStripVisible(), SetToolStripMenuItemVisible(), SetMenuStripEnabled() and 
 *                                          SetToolStripMenuItemEnabled() the Common.General class. 
 * 
 *  08/25/10    1.4     K.McD           1.  Extended the available signatures associated with the WriteStatusMessage() method to allow the programmer to specify the 
 *                                          background and foreground colours of the status message.
 * 
 *  09/30/10    1.5     K.McD           1.  Set the Checked property of the 'Configure/Enumeration' menu option to equal the state of the Enumeration property in the
 *                                          method InitializePTU().
 * 
 *                                      2.  Renamed a number of resources. No functional changes.
 * 
 *  10/10/10    1.6     K.McD           1.  Added the method CloseChildForms(). A call to this method closes any child forms that may be open.
 * 
 *  11/02/10    1.7     K.McD           1.  Added the reference to the event menu interface.
 * 
 *  11/15/10    1.8     K.McD           1.  Included catch block in method LoadWorkset() in case an exception is thrown while trying to validate the workset. 
 * 
 *  11/17/10    1.9     K.McD           1.  Modified the LoadWorkset() method as a result of replacing the WorksetManager class with the Workset class.
 * 
 *  26/11/10    1.10    K.McD           1.  Renamed the LoadWorkset() method to LoadWorksetCollection().
 *                                      2.  Added support for the worksetCollection parameter to the LoadWorksetCollection() method.
 *                                      3.  Modified the LoadDictionary() method to load all workset collections associated with the application.
 * 
 *  01/06/11    1.11    K.McD           1.  Removed the reference to the self-test menu interface from the InitializePTU() method.
 *                                      2.  Modified the SetMode() method so that the title is set to: (a) the product name, in set-up mode; (b) the product name and 
 *                                          sub-system name in on-line mode and (c) the product name and dictionary name in off-line mode.
 *                                      3.  Modified the SetMode() method to ensure that the header was updated to reflect the off-line status in off-line mode.
 *                                      4.  Modified the SetMode() method to write the car identifier associated with the current header to the car identifier status 
 *                                          label.
 *                                      5.  Now disables the File'/Open' menu options in set-up mode.
 *                                      6.  Modified the LoadWorksetCollection() method so that an InvalidDataException is thrown if the workset collection de-serialized 
 *                                          from disk is marked as invalid by the Load() method of the WorksetCollection class.
 * 
 *  01/26/11    1.12    K.McD           1.  Auto-modified as a result of name changes to a number of resources.
 * 
 *  02/14/11    1.13    K.McD           1.  Removed any references to the ISecurity interface and replaced it with a reference to the Security class.
 *                                      2.  Renamed the UpdateMainMenu() method to UpdateMenu.
 *                                      3.  Included code within the UpdateMenu() method to call the UpdateMenu() function of any child classes that are open. This 
 *                                          ensures that any form specific changes to the menu system are updated whenever the security level of the user is changed.
 * 
 *  02/28/11    1.14    K.McD           1.  Modified and added a number of XML comments.
 *                                      2.  Modified the names of a number of constants.
 *                                      3.  Added the WriteProgressBarLegend() method.
 *                                      4.  Modified the UpdateMenu() method to accommodate the changes to the menu system.
 * 
 *  03/21/11    1.15    K.McD           1.  Modified the UpdateMenu() method to use the SecurityLevelBase and SecurityLevelHighest properties of the Security class.
 *                                      2.  Added support for the Windows help engine.
 *                                      3.  Modified the code such that the Security class is not initialized until after the data dictionary has been loaded as the 
 *                                          Security class is now configured using the Security table of the data dictionary.
 * 
 *  03/28/11    1.15.1  K.McD           1.  Modified the names of a number of local variables.
 *                                      2.  Modified the LoadWorksetCollection() method to support the modified version of the WorksetCollection class.
 * 
 *  04/27/11    1.16    K.McD           1.  Modified the UpdateMenu() method to include the menu options associated with the configuration of the chart recorder and 
 *                                          setting the chart mode.
 * 
 *                                      2.  Modified the LoadAllWorksetCollections() method to load the workset collection associated with the chart recorder.
 *                                      
 *  06/22/11    1.16.1  K.McD           1.  Added support for the self test menu interface to the InitializePTU() method.
 *  
 *  07/13/11    1.16.2  K.McD           1.  Modified the following methods to take into account the redefinition of off-line mode and the addition of diagnostic mode 
 *                                          as discussed in the June sprint review: UpdateMenu(), SetMode() and LoadDataDictionary.
 *                                          
 *  07/25/11    1.16.3  K.McD           1.  Modified the SetMode() method to include support for on-line/diagnostic mode.
 *  
 *  07/27/11    1.16.4  K.McD           1.  Removed the reference to the Diagnostic mode ToolStripButton control from the SetMode() method.
 *  
 *  10/26/11    1.16.5  K.McD           1.  Auto-modified as a result of enumerator name changes. Mode.Diagnostic renamed to Mode.Configuration.
 *                                      2.  Modified the LoadWorksetCollection() method to take into account the fact that the signature of the WorksetCollection.Load() 
 *                                          method has been modified and that error reporting of workset incompatibilities is now performed in the
 *                                          WorksetCollection.Update() method.
 *                                      3.  Modified the UpdateMenu() method to ensure that the 'Set Data Dictionary' menu option is always visible, see SNCR002.41.
 *  
 *  06/04/12    1.16.6  Sean.D          1.  Add the VERSION_LENGTH constant to indicate the number of characters which are snipped off of the tail end of the version
 *                                          string to generate the default filenames for the help and database files.
 *                                      2.  Modified the call to LoadHelpFile in LoadDictionary to use the new method signature.
 *                                      3.  Modifed LoadHelpFile to attempt to use the legacy naming method if the initial attempt to access the help file fails.
 *                                      
 *  06/19/13    1.16.7  K.McD           1.  Added support for the WibuBox security device. Modified UpdateMenu() to start the m_TimerWibuBox timer when the user logs into
 *                                          the Engineering or Factory account and to stop the timer when the PTU enters the Maintenance account. The Click event of this
 *                                          timer is used to check whether the WibuBox has been removed.
 *                                          
 *                                      2.  Automatic update of the method names DirectoryManager.SetPTUCommonApplicationDataPathToDefault() and
 *                                          DirectoryManager.SetPTUCommonApplicationDataPath in the InitializeDataSubDirectories() method.
 *                                          
 *  08/05/13    1.16.8  K.McD           1.  When updating the Font property of the Parameter class in the InitializePTU() method, instead of getting the value directly
 *                                          from the Font setting, now calls the FormOptions.GetOSFont() static method to get the font setting (FontXP/FontWin7) that is
 *                                          appropriate to the operating system.
 *                                          
 *  04/08/15    1.17    K.McD           References
 *                                      1.  Upgrade the PTU software to extend the support for the R188 project as defined in purchase order
 *                                          4800010525-CU2/19.03.2015.
 *                                          
 *                                          1.  Changes to address the items outlined in the minutes of the meeting between BTPC, 
 *                                              Kawasaki Rail Car and NYTC on 12th April 2013 - MOC-0171:
 *                                              
 *                                              1.  MOC-0171-29/34. Kawasaki requested that any function that is not available to the current mode, either Maintenance
 *                                                  or Administrative, should not be displayed, or be 'greyed out'. BTPC agreed to review the software to ensure that
 *                                                  non-active menu items are �greyed out� or not shown.
 *                                              
 *                                                  As a result of the review, it is proposed that the following modifications are carried out:
 *                                              
 *                                                  1.  The 'Select Data Dictionary' menu option should only visible when logged in as a BT engineer (Factory).
 *                                                      Also when visible, it should only be enabled when not in onlide, offline or self test mode.
 *                                                      
 *                                                  2.  When in Self Test mode only the 'File/Exit', 'Help' and 'Login' main menu options should be enabled.
 *                                                  
 *                                                  3.  When displaying event logs only the File/Exit', 'Configure/Worksets/Data Stream', 'Configure/Enumeration',
 *                                                      'Help', and 'Login' main menu options should be enabled.
 *                                                          
 *                                                  4.  When the Watch Window is paused or data is being recorded only the 'File/Exit', 'Help' and 'Login' main menu
 *                                                      options should be enabled.
 *                                      Modifications
 *                                      1.  Modified the UpdateMenu() method to ensure that: (a) the 'Select Data Dictionary' menu option is disabled when in online or
 *                                          offline mode; (b) the 'Select Data Dictionary' menu option is only visible when logged into Factory mode; (c) the
 *                                          'Tools/Options' menu option is not enabled in Maintenance mode; and (d) only the 'File/Exit', 'Help' and 'Login' menu
 *                                          ptions are enabled in 'Self Test' mode.
 */

/*
 *  05/13/15    1.18    K.McD           References
 *                                      1.  Upgrade the PTU software to extend the support for the R188 project as defined in purchase order
 *                                          4800010525-CU2/19.03.2015.
 *                                          
 *                                          1.  NK-U-6505 Section 2.2. Mandatory MDI Global Screen Representation - Part 1. The proposal is to add additional status
 *                                              labels to the status bar at the bottom of the PTU screen to include �Log: [Saved | Unsaved]� and
 *                                              �WibuBox: [Present | Not Present]�.
 *                                              
 *                                      2.  SNCR - R188 PTU [20-Mar-2015] Item 4. If the PTU is being used in a development environment, i.e. there is a possibility of
 *                                          switching between multiple projects, and the PTU is started up using the R188 configuration but the user agrees to switch to
 *                                          the CTA configuration when an attempt is made to connect to a CTA VCU, there is a bug which results in the PTU continuing to
 *                                          check whether a Wibu Key is present.
 *  
 *                                      Modifications
 *                                      1.  Auto-Update as result of name changes to the status label controls. Ref.: 1.1.
 *                                      
 *                                      2.  Modified the InitializePTU() method to initialize the LogSaved flag property from the LogSaved setting. Ref.: 1.1.
 *                                      
 *                                      3.  Modified the LoadDictionary() method to check whether the current project requires a Wibu-Box and, if so, to: (a) assert the
 *                                          Visible property of the WibuBox status label; (b) instantiate a new MenuInterfaceWibuKey object; (c) initialize the WibuBox
 *                                          Timer control; and (d) check whether a valid WibuBox is connected to the USB port. 
 *                                          
 *                                          In this implementation of the WibuBox status label, the check for a valid WibuBox is only made on start-up of the PTU and
 *                                          when the user tries to log into engineering mode as it is fairly CPU intensive. Once logged into engineering mode, a
 *                                          periodic check is made to ensure that the registered WibuBox has not been removed. The check on whether a registered
 *                                          WibuBox has been removed is considerably less CPU intensive than the check to verify whether a valid WibuBox has been
 *                                          connected. Ref.: 2.                                       
 */

/*
 *  07/13/15    1.19    K.McD           References
 *                                      1.  Upgrade the PTU software to extend the support for the R188 project as defined in purchase order
 *                                          4800010525-CU2/19.03.2015.
 *                                          
 *                                          1.  NK-U-6505 Section 2.2. Mandatory MDI Global Screen Representation - Part 2.  Addition of the Control Panel window.
 *                                          
 *                                          2.  NK-U-6505 Section 2.2. Mandatory MDI Global Screen Representation - Part 1. Following a conference call on 9-Jul-15,
 *                                              the current proposal is to extend the options associated with the log saved status StatusLabel to include:
 *                                              �[Saved | Unsaved | Unknown | Not Applicable (-)]�.
 *                                          
 *                                      2.  Following the conference call on 9-Jul-15, it was decided that the Clear and Initialize event log functions should only
 *                                          be available to the Engineering account.
 *                                          
 *                                      Modifications
 *                                      1.  Added the MenuUpdated event. This event is raised when the menu is updated to reflect a new mode or security level.
 *                                          Ref.: 2.
 *                                      2.  Added a reference to a ControlPanel UserControl. This is only used if the project include a ControlPanel e.g. NYCT.
 *                                          Ref.: 1.1.
 *                                      3.  Modified the WriteCarIdentifier() method to update the CarNumber property whenever the 'Car Number' StatusLabel
 *                                          is updated. - Ref.: 1.2. 
 *                                      4.  Modified the LoadDictionary() method to initialize and add a project control panel, if appropriate to the
 *                                          current project. - Ref.: 1.1.
 *                                      5.  Modified the InitializePTU() method. Now sets the LogStatus property to EventLogSavedStatus.NotApplicable. The 
 *                                          LogSaved property and setting have been removed. Ref.: 1.2.
 *                                      6.  Added the OnMenuUpdated() method to manage the MenuUpdated event. - Ref.: 2.
 *                                      7.  Modified the UpdateMenu() method to: (a) raise the MenuUpdated event via the OnMenuUpdate() method; and (b) disable
 *                                          the initialize event logs menu option and hide the Tools menu option in Maintenance mode. - Ref.: 2.
 */

/*  
 * 07/27/15    1.20    K.McD           References
 *                                      1.  Part 1 of the upgrade to the Chicago 5000 PTU software that allows the user to download the configuration and help files for
 *                                          a particular Chicago 5000 vehicle control unit (VCU) via an ethernet connection to the FTP (File Transfer Protocol) server
 *                                          running on the VCU. The scope of Part 1 of the upgrade is defined in purchase order 4800011369-CU2 07.07.2015.
 *                                      
 *                                          The upgrade is implemented in two parts, the first part, Part 1, replaces the existing screens and logic with those outlined
 *                                          in slides 6, 7, 8 and 9 of the PowerPoint presentation '076_CTA - PTU file pullback from VCU - 20150127.pptx', but does NOT
 *                                          implement the file transfer; it merely calls an empty external batch file from within the PTU application. The second stage,
 *                                          Part 2, implements the batch file that downloads the configuration and help files from the Vehicle Control Unit (VCU) to the
 *                                          appropriate directory on the PTU computer. As described in the PowerPoint Presentation, this download is only carried out
 *                                          if the appropriate configuration file is not already present on the PTU computer.
 *                                          
 *                                      2.  An informal review of version 6.11 of the PTU concluded that, where possible - i.e. if the PTU is started from a shortcut
 *                                          that passes the project identifier as a shortcut parameter, the project specific PTU initialization should be carried out
 *                                          in the MDI Form contructor that has the parameter string array as its signature rather than by the LoadDictionary() method.
 *                                          This streamlines the display construction of the Control Panel associated with the R188 project. In the 6.11 implementation
 *                                          the CTA layout is momentarily displayed before the Control Panel is drawn, however by initializing the project specific
 *                                          features in the constructor the Control Panel associated with the R188 project is drawn immediately and the CTA layout
 *                                          is not shown at all.
 *                                      
 *                                      Modifications
 *                                      1.  Introduced a do/while statement into Program.cs statement that checks whether the Restart static flag property of the
 *                                          MDI interface is asserted and, if so, restarts the PTU application. In conjunction with this Restart property, two new methods
 *                                          have been added to the IMainWindow interface, Close() and SetRestart(). Together, these allow any class that can access the
 *                                          IMainWindow interface to initiate a restart of the PTU application. Added the IMainWindow.SetRestart() method.
 *                                          
 *                                          The reason for these changes in terms of reference 1 is that having this feature allows the, time consuming, section of
 *                                          code that disposes of the WibuBox timer and Control Panel to be removed from the LoadDictionary() method, see 4 below.
 *                                          The code was originally included to reset the PTU back to its start-up state in the event that a new data dictionary was
 *                                          selected by the user. The code is no longer required as the 'File/Select Data Dictionary' menu option now loads the data
 *                                          dictionary and restarts the PTU. - Ref.: 2.
 *                                          
 *                                      2.  Removed the WriteProgressBarLegend() methods and the TaskProgressBar ToolStripProgressBar as these no longer exist.
 *                                          The progress bar used to display the recording and playback of data streams now appears in the 'Information' Panel of
 *                                          the FormWatch Form. The progress bar was moved to allow the status message display to be extended to support some of the
 *                                          longer messages required to support the upgrade shown above. - Ref.: 1.
 *                                          
 *                                      3.  Clear the m_RestartPTU static flag in the InitializePTU() method to ensure that the application terminates when the Close()
 *                                          method is called. If this flag is asserted, the application automatically restarts when the Close() method exits. - Ref.: 2.
 *                                          
 *                                      4.  Deleted the sections of code in the LoadDictionary() method that: (a) removes the Control Panel from the list of controls
 *                                          associated with the MDI Form, if it already exists, and (b) sets the WibuBox timer and Control Panel to their start-up
 *                                          states if they have already been instantiated. The code was originally included to reset the PTU back to its start-up state
 *                                          in the event that a new data dictionary was selected by the user. The code is no longer required as the
 *                                          'File/Select Data Dictionary' menu option now loads the data dictionary and restarts the PTU. - Ref.: 2.
 *                                          
 *                                      5.  The section of code in the LoadDictionary() method that instantiated the WibuBox Timer and the Control Panel was moved
 *                                          to create the InitializePTUProjectSpecific() method. This method is responsible for initializing the project specific
 *                                          features of the PTU  e.g. the WibuBox and the Control Panel. - Ref.: 2.
 *                                          
 *                                      6.  Modified the LoadDictionary() method to check whether the project identifier was passed as a shortcut parameter and,
 *                                          if so, to check that this project identifier matches that of the data dictionary; or, if not, to call the
 *                                          InitializePTUProjectSpecific() method. - Ref.: 2.                                 
 */

/*
 *  08/11/15    1.21    K.McD       References
 *                                  1.  Changes resulting from documents: 'PTU MOC Findings - .docx' and 'PTU Installation on 64-bit Machine_v1-08022015.docx' sent
 *                                      from Atul Chaudhari on 4th Aug 2015 and the follow up email sent on 5th Aug 2015.
 *                                      
 *                                      1.  MOC-0171-005. KRC have requested that the 'Log' button is to be disabled in simulation mode to avoid confusion if the
 *                                          simulated event data were ever saved to disk.
 *                                          
 *                                      2.  On the R188 project only, all references to 'PTU' are to be replaced with 'PTE' and all occurrences of 'Portable Test Unit'
 *                                          are to be replaced with 'Portable Test Equipment'. Where support for multiple legends is not possible, 'Portable Test
 *                                          Application' is to be used as an alternative to 'Portable Test Unit'/'Portable Test Equipment.
 *                                          
 *                                      3.  The legends on the control panel buttons and labels are to be modified to match those specified in the KRC PTE Uniform
 *                                          Interface Specification Rev. B.
 *                                          
 *                                      4.  For the R188 project, the splash screen is no longer to be displayed on start-up.
 *                                          
 *                                  2.  Bug Fix - SNCR - R188 PTU [20-Mar-2015] Item 27. If the Factory user uses the �Select Data Dictionary� menu option to update
 *                                      the configuration file, or, the PTU tries to update the configuration file because of a mismatch between the version of the
 *                                      embedded software and that of the XML configuration file; the selected file is not copied across to the project default
 *                                      configuration file, consequently, the next time that the application is run; the PTU reports that it cannot locate the default
 *                                      project configuration file.
 *                                      
 *                                  3.  Bug Fix - SNCR - R188 PTU [20-Mar-2015] Item 28. If the project requires a Wibu-Key and the files �Configuration.xml� and
 *                                      �project-identifier.Configuration.xml� do not exist, there is a problem trying to log on as a Factory user in order to
 *                                      select the required data dictionary file.
 *                                          
 *                                      As the project-identifier is now passed as a desktop shortcut parameter, the Wibu-Key timer is initialized in the MDI
 *                                      constructor, if required; as soon as the user tries to log on they are automatically logged out as the initialized timer
 *                                      calls the WibuBoxCheckIfRemoved() method which returns a value of true because the FormLogin Form is instantiated without
 *                                      first calling the WibuBoxCheckForValidEntry() method as the Parameter.ProjectInformation.ProjectIdentifier parameter used
 *                                      in the call to WibuBoxCheckIfRequires() is still set to string.Empty at that stage as no data dictionary had been selected.
 *                                      
 *  
 *                                  Modifications
 *                                  1.  Modified the LoadDictionary() method to set the 'm_ProjectIdentifierPassedAsParameter' member variable to be equal to
 *                                      the project identifier associated with the selected configuration file is it is an empty string. This allows the 
 *                                      'm_ProjectIdentifierPassedAsParameter' member variable to be used to specify the project identifier when calling the
 *                                      MenuInterface.Login() method which partly resolves the problem associated with Bug Fix - SNCR - R188 PTU [20-Mar-2015] Item 28.
 *                                      Ref.: 3.
 *                                      
 *                                  2.  Modified the LoadDictionary() method only show the splash screen if it is applicable to the current project. - Ref.: 1.4.
 *                                  3.  Modified the SetMode() method to display the appropriate product name. - Ref.: 1.2.
 * 
 */

/*
 *  08/11/15    1.22    K.McD       References
 *                                  1.  Part 2 of the upgrade to the Chicago 5000 PTU software that allows the user to download the configuration and help files for
 *                                      a particular Chicago 5000 vehicle control unit (VCU) via an ethernet connection to the FTP (File Transfer Protocol) server
 *                                      running on the VCU. The scope of Part 2 of the upgrade is defined in purchase order  4800011369-CU2 28.07.2015.
 *                                      
 *                                      The upgrade is implemented in two parts, the first part, Part 1, replaces the existing screens and logic with those outlined
 *                                      in slides 6, 7, 8 and 9 of the PowerPoint presentation '076_CTA - PTU file pullback from VCU - 20150127.pptx', but does NOT
 *                                      implement the file transfer; it merely calls an empty external batch file from within the PTU application. The second stage,
 *                                      Part 2, implements the batch file that downloads the configuration and help files from the Vehicle Control Unit (VCU) to the
 *                                      appropriate directory on the PTU computer. As described in the PowerPoint Presentation, this download is only carried out if the
 *                                      appropriate configuration file is not already present on the PTU computer.
 *                                      
 *                                  Modifications
 *                                  1.  Auto-update as a result of the property and method name changes associated with Common.DirectoryManages.cs Rev. 1.4.
 *                                      1.  Auto-update: SetPTUApplicationDataPathToDefault() -> SetCommonPTUApplicationDataPathToDefault();
 *                                      2.  Auto-update:.PathDefaultPTUApplicationData -> PathDefaultCommonPTUApplicationData
 *                                      3.  Auto-update: PathPTUApplicationData -> PathCommonPTUApplicationData.
 */

/*
 *  11/16/15    1.23     K.McD      References
 *                                  1.  Added support for R179.
 *
 *                                  Modifications
 *                                  1.  Added CommonConstants.ProjectIdR179 to the switch statements in the LoadDictionary(), SetMode() and 
 *                                      InitializePTUProjectSpecific() methods.
 *                                      
 *                                  2.  Modified the code associated with the 'Project Specific Control Panel' section of the InitializePTUProjectSpecific() method
 *                                      as follows:
 *                                      
 *                                      1.  Removed the: this.SuspendLayout(), this.ResumeLayout(false) and this.PerformLayout() calls.
 *                                      2.  Added the call to this.Update().
 *                                      3.  Initialized the ProjectTitle property of the 'Control Panel'.
 */
#endregion - [1.0 to 1.23] -

#region - [1.24] -
/*
 *  03/15/2016  1.24    K.McD       References
 *                                  1.  Bug fix - SNCR - PTU [01 Mar 2016] - Item 12. There is an issue with saving fault/event logs on BART. BART has car IDs that are
 *                                      "prepended" with the letter "D" or "E" (e.g D1234, E4321). When trying to save a fault log, an exception is thrown in file
 *                                      "FormViewEventLog.cs" when executing the conditional check: 
 *                                      
 *                                          if (eventLogFile.EventRecordList[0] != Settings.Default.MostRecentDownloadedEventsSaved.DownloadedEvents[MainWindow.CarNumber])
 *                                          {
 *                                              ...
 *                                          }
 *                                          
 *                                      as MainWindow.CarNumber is not successfully converted to an integer because of the "D" or "E", hence the exception being thrown.
 *                                      
 *                                  Modifications
 *                                  1.  Modified the WriteCarIdentifier() method to strip off the leading 'D' or 'E' car type character if the Convert.ToUInt() method
 *                                      throws an exception and the project identifier is BART.
 *                                      
 *                                  2.  Changed the parameter name of the WriteCarIdentifier() method to carIdentifierString.
 */
#endregion - [1.24] -

#region - [1.25] -
/*
 * 04/07/2016   1.25    K.McD       References
 *                                  1.  Email 'D.Smail 24th March 2016 - Self Test Error Message Finding' and subsequent request in the conference call of Monday 31st
 *                                      March 2016 to modify the PTU code to inform the user of the reason why Self Tests cannot be run if the VCU fails to enter
 *                                      Self Test mode.
 *                                      
 *                                  Modifications
 *                                  1.  Added location information to the Debug.Assert statements in the SetMode() method.
 */
#endregion - [1.25] -

#region - [1.26] -
/*
 * 04/19/2016   1.26    K.McD       References
 *                                  1.  Conference Call 11th April 2016 - Add Control Panel to the BART and Toronto Rocket projects.
 *                                      
 *                                  Modifications
 *                                  1.  Modified the 'switch(projectIdentifier)' statement in the InitializePTUProjectSpecific() method to instantiate a new
 *                                      Control Panel for the BART and Toronto Rocket projects.
 *                                      
 *                                  2.  Modified the InitializePTUProjectSpecific() method to initialize the new properties associated with
 *                                      version 1.5 of the ControlPanel i.e. ControlPanelTitle, ProjectIdentifier,  WibuBoxRequired, SubsystemCode, LocationCode and
 *                                      Connection.
 */
#endregion - [1.26] -

#region - [1.27] -
/*
 * 05/16/2016   1.27        K.McD   References
 *                                  1.  BART 071-ICD-0011 PTU Requirement and Interface Description document [REQ-72] and [REQ-78]. These requirements apply to 
 *                                      Windows-based PTU applications only.
 *                                      
 *                                      1.  [REQ-72] As a minimum, the Windows based PTU shall have the access level as a command line parameter to allow third party
 *                                          software to launch it directly with the required access level.
 *                                          
 *                                          The parameter to define the access level shall be -l number with number being the access level (either "2" or "3", as
 *                                          defined in [REQ-25] and [REQ-27]).
 *                                          
 *                                          Where no access level is passed, the PTU shall query Windows for the user group that the current user belongs to (refer to
 *                                          [REQ-78]) or otherwise deny access to all functions.
 *                                          
 *                                      2.  [REQ-78] Users on the PTU laptop are members of the user group PrimaryMaintenance or Engineering. If no access level is passed
 *                                          (refer to [REQ-72]) the PTU application shall query Windows for the user group that the current user belongs to. If the user
 *                                          is part of the Engineering group, access according to Level 3 shall be granted. If the user is not part of the Engineering
 *                                          group, but part of the PrimaryMaintenance group, access according to Level 2 shall be granted. If the user is not part of
 *                                          any of those two groups, access to all functions shall be denied.
 * 
 *                                  Modifications
 *                                  1.  Modified UpdateMenu() to set the Visible property of the m_MenuItemLogin class to false if the m_HideLogin flag is asserted.
 *                                  2.  Modified LoadDictionary() to set the SecurityLevelCurrent property to the SecurityLevelPassedAsParameter property if the
 *                                      SecurityLevelPassedAsParameter property is defined.
 *                                  3.  Automatic update following changes to a number of Resource names.
 *                                  4.  Added an IsDisposed check to LoadDictionary().
 */

/*
 * 07/21/2016   1.27.1      K.McD   References
 *                                  1.  PTE Changes - List 5-17-2016.xlsx Item 4. Monitor the Wibu-Key continuously while not connected to the target logic.
 *                                  2.  PTE Changes - List 5-17-2016.xlsx Item 11. Move the 'Real Time Clock' menu option from the 'Configure' drop-down menu to the
 *                                      'Tools' drop-down menu.
 *                                  
 *                                  Modifications
 *                                  1.  Removed all references to m_TimerWibuBox from the UpdateMenu() method. - Ref.: 1. Once started, m_TimerWibuBox now runs
 *                                      continuously regardless of the current security level access.
 *                                  2.  Modified InitializePTUProjectSpecific() to start the WibuBox timer immediately after making the call to check whether
 *                                      a WibuBox was connected to the PTU.
 *                                  3.  Auto-Update -  m_MenuItemConfigureRealTimeClock renamed to m_MenuItemRealTimeClock in UpdateMenu(). - Ref.: 2.
 *                                  
 */

/*
 *  09/12/2016  1.27.2      K.McD   References
 *                                  1.  PTE Changes - List 5-17-2016.xlsx Item 15, 22, 23, 23, 25, 47, 48. Add 'Delete', 'Set As Default' and 'Override Security'
 *                                      ToolStripButton controls to the Chart Recorder, Data Stream and Watch Window configuration dialogbox forms. On selecting the
 *                                      'Delete' ToolStripButton, a pop-up asking 'Are you sure you want to delete the ...?' should appear with the option to
 *                                      answer 'Yes' or 'Cancel'.
 *                                      
 *                                  2.  Bug Fix - SNCR-PTU [01 Mar 2016] - Item 23. Only ask the user if they wish to terminate the application if they select
 *                                      the 'File/Exit' menu option or select the [x] button. If the close is as a result of a fatal error just close the program
 *                                      regardless.
 *  
 *                                  Modifications
 *                                  1.  Moved the LoadWorksetCollection() method into the [MainWindow] region as this method is now included in IMainInterface. This allows
 *                                      the 'Delete' operation on a workset to be undone as the disk file can be de-serialized to the origional 'WorkSetCollection' prior
 *                                      to the workset deletion. - Ref.: 1.
 *                                      
 *                                  2.  Modified the LoadDictionary() method to clear the DisplayQueryExit property prior to closing the form if the project identifier
 *                                      in the data dictionary does not match the project identifier that was passed as a shortcut parameter. - Ref.: 2.
 *                                      
 *                                  3.  Modified the UpdateMenu() method to reflect that the 'Configure/Worksets' and 'Configure/Chart Mode' menu options no longer
 *                                      exist. Also ensured that the 'Configure/Watch Window', 'Configure/Data Streams' and 'Configure/Chart Recorder' menu options are
 *                                      available on the 'Home' screen in 'Configuration', 'Online' and 'Offline' modes.
 */
#endregion - [1.27] -


#region - [1.28] -
/*
 * 02/25/2017   1.28.1   D.Smail        Modifications
 *                                      1.  Added code to initialize the new windows time and bacjground thread on the Main Window
 *                                      2.  Added function call so that when any MDI child form is displayed, the background
 *                                          communications thread is paused (temporaily pause communications check with
 *                                          target hardware and hand that responsibility off to the child form
 *                                      
 * 
 * 02/27/2017   1.28.2   D.Smail        Modifications
 *                                      1.  Removed PauseCommThread() and placed in each individual button click because there so much 
 *                                          CPU resources dedicated to creating/displaying the child form that the CommThread
 *                                          was "colliding" with the child form's comm thread and thus causing communication
 *                                          losses. The pausing of the Main Form comm thread couldn't occur prior to the creation
 *                                          of the child form comm thread in all instances.
 *
 * 
 */
#endregion - [1.28] -
#region - [1.29] -
/*
 *  06/24/2017     1.29.0   Vgott   References
 *                                  1.	Grey-out the Password Manager Button on the Control Panel for the BART PTU Application.  It will never be �active� for login user other than factory in the BART PTE Application.                 
 *                                  
 *                                  Modifications
 *                                  1.  Disabled  the m_MenuItemConfigurePasswordProtection menu item for password protection for BART project.
 *
 *  02/11/2018     1.29.1   Vgott   Modifications
 *                                  1.  Added project ID PAQA to don't show the splash screen and also to show the project title on Controlpanel
 *
 *  02/21/2018     1.29.2   Vgott   Modifications
 *                                  1.  Disabled the car ID functionality for Base level security login and also for the operation mode setup and configuration.
 *
 *  07/08/2018     1.29.3   Vgott   Modifications
 *                                  1.  Added project ID MAPA to don't show the splash screen and also to show the project title on Controlpanel
 *  12/06/2018     1.29.4   Vgott   Modifications
 *                                  1.  Updated the code for new project ID: 300R.    
 *                                  2.  Enable Password Manager for all the security levels.
 *                                  3.  Enable Splash screen for the 300R Project.
 *  12/21/2018     1.29.5   Vgott   Modifications
 *                                  1.  MAPA update: Enable Menu Items to set CarID and Realtime clock in Engineering and Factory level.
 */
#endregion

#endregion --- Revision History ---

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;

using Bombardier.PTU.Forms;
using Bombardier.PTU.Properties;
using Common;
using Common.Configuration;
using Common.Forms;
using Watch;
using WibuKey;
using Common.Communication;

namespace Bombardier.PTU
{
    /// <summary>
    /// Methods to support the MdiPTU class.
    /// </summary>
    public partial class MdiPTU
    {
        #region --- Events ---
        /// <summary>
        /// Raised when the menu is updated to reflect a new mode or security level.
        /// </summary>
        public event EventHandler MenuUpdated;
        #endregion --- Events ---

        #region --- Constants ---
        /// <summary>
        /// The number of characters which are snipped off of the tail end of the version string to generate the default
        /// filenames for the help and database files. Methodology is a legacy of the DDB tool.
        /// </summary>
        private const int VERSION_LENGTH = 4;
        #endregion --- Constants ---

        #region --- Member Variables ---
        /// <summary>
        /// The default background color associated with the status message and progress bar legend. Value: Color.WhiteSmoke.
        /// </summary>
        private readonly Color WriteDefaultBackColor = Color.FromKnownColor(KnownColor.Transparent);

        /// <summary>
        /// The default foreground color associated with the status message and progress bar legend. Value: Color.Black.
        /// </summary>
        private readonly Color WriteDefaultForeColor = Color.Black;

        /// <summary>
        /// Reference to the Control Panel UserControl if one is used on the project.
        /// </summary>
        private ControlPanel m_ControlPanel = null;
        #endregion --- Member Variables ---

        #region - [MainWindow] -
        /// <summary>
        /// Sets the static flag that controls whether the PTU does an automatic restart when the main PTU application is closed. True, if the PTU is to do
        /// an automatic restart; otherwise, false.
        /// </summary>
        public void SetRestart(bool value)
        {
            m_Restart = value;
        }

        /// <summary>
        /// Close any child form that may be open. For this to work the child form must inherit from <c>FormPTU</c>.
        /// </summary>
        /// <remarks>The child forms are closed cleanly by simulating the user having pressed the escape key associated with the form.</remarks>
        public void CloseChildForms()
        {
            // Close any child forms that are open.
            int mdiChildren = MdiChildren.Length;
            if (MdiChildren.Length > 0)
            {
                // Close the child forms in reverse order as it is aesthetically better.
                for (int childIndex = mdiChildren; childIndex > 0; childIndex--)
                {
                    // Ensure that the child form exits cleanly.
                    (MdiChildren[childIndex - 1] as FormPTU).Exit();
                    Update();
                }
            }
        }

        /// <summary>
        /// Write the specified car identifier string in the status label used to display the car identifier, convert the car identifier string to a numeric value
        /// and update the CarNumber property.
        /// </summary>
        /// <remarks>The car identifier string on BART includes a leading 'D' or 'E' car type identifier, it is necessary to remove this before attempting to
        /// convert the car identifier string to a numeric value, otherwise an exception is thrown.</remarks>
        /// <param name="carIdentifierString">The car identifier string. Valid values are from "0000" to "9999". All 4 digits must be specified.</param>
        public void WriteCarIdentifier(string carIdentifierString)
        {
            // The car number as a short. If the specified car number string is valid, the value will be >= 0 and <= 9999; otherwise, it will be set to short.MinVal.
            UInt16 carNumberAsUInt16;

            m_StatusLabelCarNumber.Text = Resources.LegendCarNumber + CommonConstants.Space + carIdentifierString;
            Update();

            // Convert the car identifier string to a numeric UInt16 value.
            try
            {
                carNumberAsUInt16 = Convert.ToUInt16(carIdentifierString);
            }
            catch (Exception)
            {
                // Check whether the car identifier is associated with the BART project as the car identifier may be 'prepended' with either a 'D' or 'E' character to
                // identify the car type.
                switch (Parameter.ProjectInformation.ProjectIdentifier)
                {
                    case CommonConstants.ProjectIdBART:
                        try
                        {
                            // As the car identifier on BART is prepended with a leading 'D' or 'E' car type identifier it is necessary to remove this before attempting to
                            // convert the identifier to a string.
                            carNumberAsUInt16 = Convert.ToUInt16(carIdentifierString.Trim().Remove(0, 1));
                        }
                        catch (Exception)
                        {
                            carNumberAsUInt16 = UInt16.MinValue;
                        }
                        break;
                    default:
                        carNumberAsUInt16 = UInt16.MinValue;
                        break;
                }
            }

            // Check whether the car number is valid and update the CarNumber property accordingly.
            CarNumber = ((carNumberAsUInt16 >= 0) && (carNumberAsUInt16 <= CarNumberMax)) ? carNumberAsUInt16 : UInt16.MinValue;
        }

        /// <summary>
        /// Blink the data update icon - this indicates that data has been received from the target hardware.
        /// </summary>
        public void BlinkUpdateIcon()
        {
            m_DigitalControlPacketReceived.BlinkThreadSafe();
            Update();
        }

        /// <summary>
        /// Show that the security level has been updated by modifying: (a) the Login/Logout text associated with the menu and button; (b) the status line text 
        /// and (c) the menu options to reflect the new clearance level.
        /// </summary>
        /// <param name="security">Reference to the security class for which the security clearance level is to be displayed.</param>
        public void ShowSecurityLevelChange(Security security)
        {
            // If the user logs in, change the 'Login' menu option text to 'Logout'.
            if (security.SecurityLevelCurrent <= m_Security.SecurityLevelBase)
            {
                m_MenuItemLogin.Text = Resources.SecurityLogIn;
            }
            else
            {
                m_MenuItemLogin.Text = Resources.SecurityLogOut;
            }

            // Update the status information.
            m_StatusLabelSecurityLevel.Text = security.Description;

            // Update the menu options to reflect the new security level.
            UpdateMenu(security);
        }

        /// <summary>
        /// Write the specified message to the status message control using the default <c>BackColor</c> and <c>ForeColor</c> properties.
        /// </summary>
        public void WriteStatusMessage(string message)
        {
            m_StatusLabelMessage.BackColor = WriteDefaultBackColor;
            m_StatusLabelMessage.ForeColor = WriteDefaultForeColor;
            m_StatusLabelMessage.Text = CommonConstants.Space + message;
            Update();
        }

        /// <summary>
        /// Write the specified message to the status message control using the specified <c>BackColor</c> and <c>ForeColor</c> properties.
        /// </summary>
        /// <param name="message">The message that is to be written.</param>
        /// <param name="backColor">The background color of the message.</param>
        /// <param name="foreColor">The foreground color of the message.</param>
        public void WriteStatusMessage(string message, Color backColor, Color foreColor)
        {
            m_StatusLabelMessage.BackColor = backColor;
            m_StatusLabelMessage.ForeColor = foreColor;
            m_StatusLabelMessage.Text = CommonConstants.Space + message;
            Update();
        }

        ///<summary>
        /// Show the specified child form.
        ///</summary>
        ///<param name="childForm">The child form that is to be displayed.</param>
        public void ShowMdiChild(FormPTU childForm)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            childForm.MdiParent = this;
            childForm.Show();
        }

        ///<summary>
        /// Show the specified dialog form.
        ///</summary>
        ///<param name="dialogForm">The child form that is to be displayed.</param>
        public DialogResult ShowDialog(FormPTUDialog dialogForm)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return DialogResult.Cancel;
            }

            dialogForm.CalledFrom = this;
            DialogResult dialogResult = dialogForm.ShowDialog();
            return dialogResult;
        }

        /// <summary>
        /// <para>Configures the PTU application using the specified data dictionary. The configuration procedure is as follows:</para>
        /// <para>(1) updates the <c>Parameter</c> class;</para>
        /// <para>(2) updates the Lookup static class to allow the records contained within the primary key data tables of the data dictionary to be accessed;</para> 
        /// <para>(3) creates the application data sub-directories if they do not exist;</para>
        /// <para>(4) updates the main menu options to reflect the current project and security level;</para>
        /// <para>(5) updates the file header information;</para>
        /// <para>(6) loads the default worksets associated with the specified data dictionary, if they exist;</para>
        /// <para>(7) loads the help file associated with the current project and software version;</para>
        /// <para>(8) shows the 'Help/About' screen.</para>
        /// </summary>
        /// <param name="dataDictionary">The data dictionary that is to be used to configure the PTU.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dataDictionary"/> is null.</exception>
        public void LoadDictionary(DataDictionary dataDictionary)
        {
            // Skip, if the Dispose() method has been called.
            if (IsDisposed)
            {
                return;
            }

            Debug.Assert(dataDictionary != null, "MdiPTU.Support.LoadDictionary - [dataDictionary != null]");

            // The configuration file was OK, initialize the Parameter class.
            Parameter.Initialize(dataDictionary);

            // Check whether the project identifier was passed as a shortcut parameter.
            if (m_ProjectIdentifierPassedAsParameter.Equals(string.Empty))
            {
                // No, initialize the project specific features of the PTU application.
                InitializePTUProjectSpecific(Parameter.ProjectInformation.ProjectIdentifier);

                // Update the member variable that records the project-identifier that was passed as a parameter with the project-identifier associated with the
                // selected configuration file.
                m_ProjectIdentifierPassedAsParameter = Parameter.ProjectInformation.ProjectIdentifier;
            }
            else
            {
                // Check whether the data dictionary matches the project identifier that was passed as a shortcut parameter.
                if (Parameter.ProjectInformation.ProjectIdentifier != m_ProjectIdentifierPassedAsParameter)
                {
                    MessageBox.Show(Resources.MBTConfigProjectIDMismatch, Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DisplayQueryExit = false;
                    Close();
                    return;
                }
            }

            Lookup.Initialize(dataDictionary);
            
            InitializeDataSubDirectories();

            // Initialize the system security and set the security clearance to the base level.
            Security.Initialize();
            m_Security = new Security();

            // If the start-up security level was passed as a command line parameter, update the current security level.
            if (m_SecurityLevelPassedAsParameter != SecurityLevel.Undefined)
            {
                m_Security.SecurityLevelCurrent = SecurityLevelPassedAsParameter;
            }

            ShowSecurityLevelChange(m_Security);

            // Update the FileHeader.HeaderCurrent property with the parameter values that are currently known.
            Header_t header = new Header_t();
            FileHeader.Initialize(ref header);
            header.ProjectInformation = Parameter.ProjectInformation;
            FileHeader.HeaderCurrent = header;

            SetMode(Mode.Configuration);
            LoadAllWorksetCollections(Parameter.ProjectInformation.ProjectIdentifier);
			LoadHelpFile(Parameter.ProjectInformation.Version, Parameter.ProjectInformation.ProjectIdentifier);

            // If applicable to the current project, show the splash screen.
            switch (m_ProjectIdentifierPassedAsParameter)
            {
                case CommonConstants.ProjectIdNYCT:
                case CommonConstants.ProjectIdR179:
                case CommonConstants.ProjectIdPAQA:
                case CommonConstants.ProjectIdMAPA:               
                    // Do not show the splash screen for the R188 project.
                    break;
                default:
                    // Show the splash screen.
                    using (FormHelpAbout formHelpAbout = new FormHelpAbout(Resources.ProductNamePTU))
                    {
                        formHelpAbout.ShowDialog();
                    }
                    break;
            }
        }

        /// <summary>
        /// Set the current mode of operation. This will set the on-line/off-line buttons; status line text and menu options to reflect the specified mode.
        /// </summary>
        /// <param name="mode">The new mode of operation.</param>
        public void SetMode(Mode mode)
        {
            //Keep a record of the current mode.
            m_Mode = mode;

            // The product name that is to be used. This is project dependent and can be either 'Portable Test Unit' or 'Portable Test Equipment'.
            string productName;
            switch (Parameter.ProjectInformation.ProjectIdentifier)
            {
                case "":
                    productName = Application.ProductName;
                    break;
                case CommonConstants.ProjectIdNYCT:
                case CommonConstants.ProjectIdR179:
                    productName = Resources.ProductNamePTE;
                    break;
                default:
                    productName = Resources.ProductNamePTU;
                    break;
            }

            if (mode == Mode.Setup)
            {
                m_TSBOnline.Enabled = false;
                m_TSBOnline.Checked = false;

                m_TSBOffline.Enabled = false;
                m_TSBOffline.Checked = false;

                m_StatusLabelMode.BackColor = Color.FromKnownColor(KnownColor.Control);
                m_StatusLabelMode.Text = Resources.LabelModeSetup;

                // Show only the product name on the title.
                this.Text = Application.ProductName;
            }
            else if (mode == Mode.Configuration)
            {
                m_TSBOnline.Enabled = true;
                m_TSBOnline.Checked = false;

                m_TSBOffline.Enabled = true;
                m_TSBOffline.Checked = false;

                m_StatusLabelMode.BackColor = Color.FromKnownColor(KnownColor.Control);
                m_StatusLabelMode.Text = Resources.LabelModeDiagnostic;
                m_StatusLabelMode.Image = Resources.ModeDiagnostic;

                // Update the header to reflect the diagnostic mode.
                Header_t header = FileHeader.HeaderCurrent;
                header.SetToDiagnostic();
                FileHeader.HeaderCurrent = header;

                // Consider the case where the data dictionary name has not yet been initialized.
                if (FileHeader.HeaderCurrent.ProjectInformation.DataDictionaryName == null)
                {
                    // Show only the product name on the title.
                    this.Text = Application.ProductName;
                }
                else
                {
                    // Update the title with the data dictionary name retrieved from the configuration file.
                    this.Text = productName + CommonConstants.BindingMessage + FileHeader.HeaderCurrent.ProjectInformation.DataDictionaryName;
                }
            }
            else if (mode == Mode.Online)
            {
                Debug.Assert(CommunicationInterface != null, "MdiPTU.Support.SetMode(Mode.Online) - CommunicationInterface != null");
                m_TSBOnline.Enabled = true;
                m_TSBOnline.Checked = true;

                m_TSBOffline.Enabled = false;
                m_TSBOffline.Checked = false;

                m_StatusLabelMode.BackColor = Color.LightGreen;
                m_StatusLabelMode.Text = Resources.LabelModeOnline;
                m_StatusLabelMode.Image = Resources.ModeOnline.ToBitmap();

                // Update the title with the sub-system name retrieved from the target. 
                this.Text = Application.ProductName + CommonConstants.BindingMessage + FileHeader.HeaderCurrent.TargetConfiguration.SubSystemName;
            }
            else if (mode == Mode.Offline)
            {
                m_TSBOnline.Enabled = false;
                m_TSBOnline.Checked = false;

                m_TSBOffline.Enabled = true;
                m_TSBOffline.Checked = true;

                m_StatusLabelMode.BackColor = Color.LightSteelBlue;
                m_StatusLabelMode.Text = Resources.LabelModeOffline;
                m_StatusLabelMode.Image = Resources.ModeOffline.ToBitmap();

                // Update the title with the sub-system name retrieved from the target.
                this.Text = productName + CommonConstants.BindingMessage + FileHeader.HeaderCurrent.TargetConfiguration.SubSystemName;
            }
            else if (mode == Mode.SelfTest)
            {
                Debug.Assert(CommunicationInterface != null, "MdiPTU.Support.SetMode(Mode.SelfTest) - CommunicationInterface != null");
                m_TSBOnline.Enabled = false;
                m_TSBOnline.Checked = true;

                m_TSBOffline.Enabled = false;
                m_TSBOffline.Checked = false;

                m_StatusLabelMode.BackColor = Color.Red;
                m_StatusLabelMode.Text = Resources.LabelModeSelfTest;
                m_StatusLabelMode.Image = Resources.SelfTest;

                // Update the title with the sub-system name retrieved from the target. 
                this.Text = productName + CommonConstants.BindingMessage + FileHeader.HeaderCurrent.TargetConfiguration.SubSystemName;
            }
            else
            {
                throw new ArgumentException("Invalid mode.", "mode");
            }

            // Update the car identifier and the title.
            WriteCarIdentifier(FileHeader.HeaderCurrent.TargetConfiguration.CarIdentifier);

            m_StatusStrip.Refresh();

            UpdateMenu(m_Security);
        }

        /// <summary> 
        /// Load the specified workset collection file from disk and then set the active workset to be the default workset. If the workset collection 
        /// file does not exist or is corrupt, then an empty workset collection file is created.
        /// </summary>
        /// <remarks>
        /// The workset collection filename is derived as follows: {project identifier}.{workset collection type}.{extension} and it is assumed that it is located in 
        /// the 'DirectoryManager.PathwWorksetFiles' directory.
        /// </remarks>
        /// <param name="worksetCollection">The workset collection that is to be loaded from disk.</param>
        /// <param name="projectIdentifier">The project identifier used to generate the workset filename.</param>
        public void LoadWorksetCollection(WorksetCollection worksetCollection, string projectIdentifier)
        {
            Debug.Assert(projectIdentifier != string.Empty, "MdiPTU.Support.LoadWorksetCollection - [projectIdentifier != string.Empty]");
            Debug.Assert(worksetCollection != null, "MdiPTU.Support.LoadWorksetCollection - [worksetCollection != null]");

            // Keep a record of the base parameters so that a new workset collection can be re-generated if the attempt to load the workset collection from disk fails.
            WorksetCollectionType worksetCollectionType = worksetCollection.WorksetCollectionType;
            short entryCountMax = worksetCollection.EntryCountMax;
            short columnCountMax = worksetCollection.ColumnCountMax;

            // ----------------------------------------------------------------------------------
            // Attempt to load the workset file associated with the specified project identifier.
            // ----------------------------------------------------------------------------------
            string qualifier = "." + worksetCollection.WorksetCollectionType.ToString();

            // Generate the name of the file containing the serialized workset information i.e. [ProjectID].set.
            string filename = projectIdentifier + qualifier + CommonConstants.ExtensionWorksetFile;

            // Generate the fully qualified file name of the file containing the serialized data. 
            string fullFilename = DirectoryManager.PathWorksetFiles + CommonConstants.BindingFilename + filename;

            try
            {
                //Deserialize the specified workset collection file.
                worksetCollection.Load(fullFilename);
            }
            catch (SerializationException serializationException)
            {
                worksetCollection.Initialize(worksetCollectionType, entryCountMax, columnCountMax);
                throw new SerializationException(serializationException.Message);
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                worksetCollection.Initialize(worksetCollectionType, entryCountMax, columnCountMax);
                throw new FileNotFoundException(fileNotFoundException.Message);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                worksetCollection.Initialize(worksetCollectionType, entryCountMax, columnCountMax);
                throw new UnauthorizedAccessException(unauthorizedAccessException.Message);
            }

            // Ensure that the default workset is selected.
            worksetCollection.Reset();
        }
        #endregion - [MainWindow] -

        #region - [Initialization] -
        /// <summary>
        /// <para>Initialize the project specific features of the PTU application:</para>
        /// <para>(1) check whether a WibuBox is required for the project and, if so, initialize it;</para>
        /// <para>(2) check whether the project uses a control panel and, if so, initialize it.</para>
        /// </summary>
        /// <param name="projectIdentifier">The project identifier string that was passed as a shortcut parameter.</param>
        private void InitializePTUProjectSpecific(string projectIdentifier)
        {
            #region - [WibuBox] -
            // Check whether the project requires a WibuBox security device.
            if (m_MenuInterfaceApplication.WibuBoxCheckIfRequired(projectIdentifier) == true)
            {
                // Yes, set up the WibuBox as follows: (a) set the WibuBox status label 'Visible' property to be true; (b) instantiate the WibuBox menu interface;
                // (c) initialize the WibuBox Timer control; and (d) check whether a valid WibuBox is connected to the USB port.
                try
                {
                    // Set the WibuBox status label 'Visible' property to be true.
                    m_StatusLabelWibuBoxStatus.Visible = true;

                    // Initialize the WibuBox timer. The event handler associated with this timer checks that a valid WibuBox
                    // security device is attached to the system. If it detects that the device has been removed it will set 
                    // the current security level to the lowest security access level.
                    m_TimerWibuBox = new Timer();
                    m_TimerWibuBox.Tick += new EventHandler(WibuBoxCheck);
                    m_TimerWibuBox.Interval = IntervalMsWibuBoxUpdate;
                    m_TimerWibuBox.Enabled = true;
                    m_TimerWibuBox.Stop();

                    // Check whether a valid WibuBox is connected to the USB port and update the status label.
                    m_MenuInterfaceWibuKey = new MenuInterfaceWibuKey(this);
                    WibuBoxPresent = m_MenuInterfaceWibuKey.WibuBoxCheckForValidEntry(true);

                    m_TimerWibuBox.Start();
                }
                catch (Exception)
                {
                    MessageBox.Show(Resources.MBTWibuKeyDeviceDriverNotInstalled + CommonConstants.NewPara + Resources.MBTWibuKeyInstall, Resources.MBCaptionError,
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            #endregion - [WibuBox] -

            #region - [Project Specific Control Panel] -
            // The NYCT project uses a Control Panel to activate the various menu options.
            switch (projectIdentifier)
            {
                case CommonConstants.ProjectIdNYCT:
                case CommonConstants.ProjectIdR179:
                case CommonConstants.ProjectIdBART:
                case CommonConstants.ProjectIdRocket:
                case CommonConstants.ProjectIdPAQA:
                case CommonConstants.ProjectIdMAPA:
                case CommonConstants.ProjectIdAPM:
                    m_ControlPanel = new ControlPanel();
                    m_ControlPanel.Dock = DockStyle.Left;
                    m_ControlPanel.Parent = this;
                    m_ControlPanel.TabStop = false;
                    m_ControlPanel.TabIndex = 0;
                    m_ControlPanel.Name = CommonConstants.KeyControlPanel;
                    m_ControlPanel.ProjectIdentifier = projectIdentifier;

                    switch (projectIdentifier)
                    {
                        case CommonConstants.ProjectIdNYCT:
                            m_ControlPanel.ControlPanelTitle = Resources.TextProjectTitleR188;
                            break;
                        case CommonConstants.ProjectIdR179:
                            m_ControlPanel.ControlPanelTitle = Resources.TextProjectTitleR179;
                            break;
                        case CommonConstants.ProjectIdBART:
                            m_ControlPanel.ControlPanelTitle = Resources.TextProjectTitleBART;
                            break;
                        case CommonConstants.ProjectIdRocket:
                            m_ControlPanel.ControlPanelTitle = Resources.TextProjectTitleTorontoRocket;
                            break;
                        case CommonConstants.ProjectIdPAQA:
                            m_ControlPanel.ControlPanelTitle = Resources.TextProjectTitlePAQA;
                            break;
                        case CommonConstants.ProjectIdMAPA:
                            m_ControlPanel.ControlPanelTitle = Resources.TextProjectTitleMAPA;
                            break;
                        case CommonConstants.ProjectIdAPM:
                            m_ControlPanel.ControlPanelTitle = Resources.TextProjectTitleAPM;
                            break;
                        default:
                            m_ControlPanel.ControlPanelTitle = string.Empty;
                            break;
                    }

                    // Specify whether the control panel needs to support the WibuBox security system.
                    if (m_MenuInterfaceApplication.WibuBoxCheckIfRequired(projectIdentifier) == true)
                    {
                        m_ControlPanel.WibuBoxRequired = true;
                    }
                    else
                    {
                        m_ControlPanel.WibuBoxRequired = false;
                    }

                    m_ControlPanel.SubsystemCode = string.Empty;;
                    m_ControlPanel.LocationCode = string.Empty;
                    m_ControlPanel.Connection = string.Empty;

                    m_ControlPanel.InitializeControlPanel(this as IMainWindow);
                    this.Controls.RemoveByKey(CommonConstants.KeyPanelStatus);
                    this.Controls.RemoveByKey(CommonConstants.KeyMenuStrip);
                    this.Controls.RemoveByKey(CommonConstants.KeyToolStripFunctionKeys);
                    this.Controls.Add(this.m_ControlPanel);
                    this.Controls.Add(this.m_ToolStripFunctionKeys);
                    this.Controls.Add(this.m_MenuStrip);
                    this.Controls.Add(this.m_PanelStatus);
                    this.Update();
                    break;
                default:
                    break;
            }
            #endregion - [Project Specific Control Panel] -
        }

        /// <summary>
        /// <para>Initialize the PTU application:</para>
        /// <para>(1) position the form;</para> 
        /// <para>(2) update the LogStatus StatusLabel, regardless of whether this is visible for the current project;</para>
        /// <para>(3) set the font;</para>
        /// <para>(4) instantiate the menu interfaces to the various sub-systems;</para>
        /// <para>(5) create the required PTU configuration sub-directories if they don't already exist;</para>
        /// <para>(6) initialize the menu system;</para>
        /// <para>(7) set the 'Checked' property of the 'Configure/Enumeration' menu option; and</para>
        /// <para>(8) clear the Restart property to ensure that the program terminates when the Close() method is called.</para>
        /// </summary>
        private void InitializePTU()
        {
            // Display the form using the saved settings.
            this.WindowState = Settings.Default.WindowState;
            this.Location = Settings.Default.FormLocation;
            this.Size = Settings.Default.FormSize;

            // Update the LogStatus StatusStrip even though it may be invisible on some projects.
            LogStatus = EventLogSavedStatus.NotApplicable;

            // Update the Font.
            Parameter.Font = FormOptions.GetOSFont(false);
            Font = Parameter.Font;

            // Menu interfaces.
            m_MenuInterfaceApplication = new MenuInterfaceApplication(this);
            m_MenuInterfaceWatch = new MenuInterfaceWatch(this);
            m_MenuInterfaceEvent = new Event.MenuInterfaceEvent(this);
            m_MenuInterfaceSelfTest = new SelfTest.MenuInterfaceSelfTest(this);

            // Create the PTU configuration sub-directories.
            DirectoryManager.CreateConfigurationSubDirectories();

            // Get the collection of function keys associated with the form. This allows any child form that is called indirectly to restore the function
            // keys on exit.
            m_ToolStripItemCollectionMainWindow = General.GetToolStripItemCollection(m_ToolStripFunctionKeys);

            // Ensure that the application terminates when the Close() method is called. If this flag is asserted, the application automatically restarts when the
            // Close() method exits.
            m_Restart = false;

            // Start Target communication timer and thread
            InitCommTimer();
        }


        /// <summary>
        /// Load all workset collections associated with the specified project identifier from disk. The filename of the file containing the workset collection is 
        /// derived from the project identifier as follows: {project identifier}.{workset collection type}.{extension}.
        /// </summary>
        /// <param name="projectIdentifier">The project identifier associated with the data dictionary.</param>
        private void LoadAllWorksetCollections(string projectIdentifier)
        {
            Debug.Assert(projectIdentifier != string.Empty, "MdiPTU.Support.LoadAllWorksetCollections - [projectIdentifier != string.Empty]");

            Workset.Initialize();

            // Check that the project identifier has been defined, this partly defines the filename of the workset collection file.
            if (Parameter.ProjectInformation.ProjectIdentifier != string.Empty)
            {
                // Load the worksets associated with viewing and recording watch variables.
                try
                {
                    LoadWorksetCollection(Workset.RecordedWatch, Parameter.ProjectInformation.ProjectIdentifier);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(Resources.MBTWorksetCollectionWatchLoadFailed + CommonConstants.NewLine + CommonConstants.NewLine + exception.Message,
                                    Resources.MBCaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Load the worksets associated with the fault log data streams.
                try
                {
                    LoadWorksetCollection(Workset.FaultLog, Parameter.ProjectInformation.ProjectIdentifier);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(Resources.MBTWorksetCollectionFaultLogLoadFailed + CommonConstants.NewLine + CommonConstants.NewLine + exception.Message,
                                    Resources.MBCaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Load the worksets associated with the chart recorder.
                try
                {
                    LoadWorksetCollection(Workset.ChartRecorder, Parameter.ProjectInformation.ProjectIdentifier);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(Resources.MBTWorksetCollectionChartRecorderLoadFailed + CommonConstants.NewLine + CommonConstants.NewLine + exception.Message,
                                    Resources.MBCaptionInformation, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(Resources.MBTDataDictionaryProjectIdEmpty, Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load the help file associated with the specified project version reference.
        /// </summary>
        /// <remarks>
        /// The help filename is derived as follows: '{project version reference}.hlp' and it is assumed that it is located in the 'DirectoryManager.' 
        /// directory.
        /// </remarks>
        /// <param name="version">The project version reference, used to generate the filename of the help file.</param>
        /// <param name="projectIdentifier">The project identifier code</param>
		private void LoadHelpFile(string version, string projectIdentifier)
        {
            // Generate the default name of the diagnostic help file.
			string filename = projectIdentifier + version.Substring(version.Length - VERSION_LENGTH, VERSION_LENGTH) + CommonConstants.ExtensionHelpFile;

            // Generate the fully qualified filename of the diagnostic help file.
            string fullyQualifiedHelpFilename = DirectoryManager.PathDiagnosticHelpFiles + CommonConstants.BindingFilename + filename;

			FileInfo helpFileInfo = new FileInfo(fullyQualifiedHelpFilename);

			if (!helpFileInfo.Exists)
			{
				// Try using the whole version string for reverse compatibility with the prior releases.

				// Generate the default name of the diagnostic help file.
				filename = version + CommonConstants.ExtensionHelpFile;

				// Generate the fully qualified filename of the diagnostic help file.
				fullyQualifiedHelpFilename = DirectoryManager.PathDiagnosticHelpFiles + CommonConstants.BindingFilename + filename;
			}

            try
            {
                WinHlp32.Initialize(fullyQualifiedHelpFilename);
            }
			catch (ArgumentException)
            {
                // Error in the fullyQualifiedHelpFilename. Either the file does not exist or it has the wrong extension.
                MessageBox.Show(Resources.MBTDiagnosticHelpNotFound, Resources.MBCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Determines the configured PTU application data directory path and: (a) creates the PTU application data directory, if it does not exist; (b) creates the PTU
        /// application data sub-directories, if they do not exist and (c) resets the initial directory paths associated with reading and writing data from/to the 
        /// PTU application data directories to their default values.
        /// </summary>
        /// <remarks>Uses the static classes Parameter and DirectoryManager.</remarks>
        private void InitializeDataSubDirectories()
        {
            // Local reference to the DirectoryInfo class.
            DirectoryInfo directoryInfo;

            // Check the specified PTU application data directory.
            if ((Parameter.PathPTUApplicationData == string.Empty) || (Parameter.PathPTUApplicationData.Equals(Resources.PathUseDefault)))
            {
                // Use the Windows default.
                DirectoryManager.SetCommonPTUApplicationDataPathToDefault();
            }
            else if (Parameter.PathPTUApplicationData.Contains(Resources.PathUseDefault))
            {
                // Strip off the <default> mnemonic in order to determine the specified sub-directory.
                string subDirectory = Parameter.PathPTUApplicationData.Replace(Resources.PathUseDefault + CommonConstants.BindingDirectory, string.Empty);
                string specifiedPath = DirectoryManager.PathDefaultCommonPTUApplicationData + CommonConstants.BindingDirectory + subDirectory;
                // Check that the specified directory is valid.
                try
                {
                    directoryInfo = new DirectoryInfo(specifiedPath);
                    // If the specified directory doesn't exist try creating it.
                    if (!directoryInfo.Exists)
                    {
                        // Throws a DirectoryNotFoundException if the specified directory cannot be created.
                        directoryInfo.Create();
                    }
                    // Set the path specified in the configuration file to be the PTU application data directory.
                    DirectoryManager.SetCommonPTUApplicationDataPath(specifiedPath);
                }
                catch (Exception)
                {
                    // Use the Windows default.
                    MessageBox.Show(Resources.MBTDataDictionaryApplicationDataPathInvalid, Resources.MBCaptionWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // User specified directory, Check that it is valid.
                try
                {
                    directoryInfo = new DirectoryInfo(Parameter.PathPTUApplicationData);
                    // If the specified directory doesn't exist try creating it.
                    if (!directoryInfo.Exists)
                    {
                        // Throws a DirectoryNotFoundException if the specified directory cannot be created.
                        directoryInfo.Create();
                    }
                    // Set the path specified in the configuration file to be the PTU application-specific data directory.
                    DirectoryManager.SetCommonPTUApplicationDataPath(Parameter.PathPTUApplicationData);
                }
                catch (DirectoryNotFoundException)
                {
                    // Use the Windows default.
                    MessageBox.Show(Resources.MBTDataDictionaryApplicationDataPathInvalid, Resources.MBCaptionWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // Check whether the PTU application data directory exists and, if not, create it.
            directoryInfo = new DirectoryInfo(DirectoryManager.PathCommonPTUApplicationData);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            // Check whether the sub-directories associated with the PTU application data directory have been created and, if not, create them.
            DirectoryManager.CreateDataSubDirectories();

            // Reset the initial directory paths associated with reading and writing data from/to the PTU application data directories to their default values.
            InitialDirectory.Reset();
        }
        #endregion - [Initialization] -

        #region - [Menu Setup] -
        /// <summary>
        /// Update the main menu options to reflect the mode and security level.
        /// </summary>
        public void UpdateMenu(Security security)
        {
            Debug.Assert(security != null);

            // Initially assert the Visible and Enabled properties for ALL menu options.
            General.SetMenuStripVisible(m_MenuStrip, true);
            General.SetMenuStripEnabled(m_MenuStrip, true);

            if (m_HideLogin == true)
            {
                m_MenuItemLogin.Visible = false;
            }

            #region - [Mode] -
            // Disable those menu options that are not applicable to the current mode of operation.
            switch (m_Mode)
            {
                case Mode.Setup:
                    // Also add all menu items defined for off-line mode.
                    m_MenuItemFileOpen.Enabled = false;
                    m_MenuItemView.Enabled = false;
                    m_MenuItemDiagnostics.Enabled = false;
                    m_MenuItemRealTimeClock.Enabled = false;
                    m_MenuItemCarId.Enabled = false;
                    m_MenuItemConfigureWatchWindow.Enabled = false;
                    m_MenuItemConfigureDataStream.Enabled = false;
                    m_MenuItemConfigureChartRecorder.Enabled = false;
                    break;
                case Mode.Configuration:
                    m_MenuItemView.Enabled = false;
                    m_MenuItemDiagnostics.Enabled = false;
                    m_MenuItemCarId.Enabled = false;
                    m_MenuItemRealTimeClock.Enabled = false;
                    break;
                case Mode.Online:
                    m_MenuItemFileSelectDataDictionary.Enabled = false;
                    break;
                case Mode.Offline:
                    m_MenuItemFileSelectDataDictionary.Enabled = false;
                    break;
                case Mode.SelfTest:
                    m_MenuItemFileOpen.Enabled = false;
                    m_MenuItemFileSelectDataDictionary.Enabled = false;
                    m_MenuItemView.Enabled = false;
                    m_MenuItemDiagnostics.Enabled = false;
                    m_MenuItemConfigure.Enabled = false;
                    m_MenuItemTools.Enabled = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("m_Mode", "MdiPTU.Support.UpdateMenu()");
            }
            #endregion - [Mode] -]

            #region - [Security] -
            if ((short)security.SecurityLevelCurrent > (short)security.SecurityLevelHighest)
            {
                // ----------------
                // System Developer
                // ----------------

                // Leave the Visible/Enabled properties of all menu options asserted.
            }
            else if ((short)security.SecurityLevelCurrent == (short)security.SecurityLevelHighest)
            {
                // ------------------------------------------------
                // Highest Security Level Appropriate to the Client
                // ------------------------------------------------

                // Clear the Visible/Enabled properties of those menu options, including separators, that are not applicable to client engineers.

                // - [File] -
                m_MenuItemFileSelectDataDictionary.Visible = false;

                // - [Configure] -
                if (Parameter.ProjectInformation.ProjectIdentifier != CommonConstants.ProjectIdMAPA)
                {
                    m_MenuItemRealTimeClock.Visible = false;
                    m_SeparatorRealTimeClock.Visible = false;
                    m_MenuItemCarId.Visible = false;
                    m_SeparatorCarId.Visible = false;
                }
                // - [Tools] -
                m_MenuItemToolsConvertEngineeringFile.Visible = false;
                m_SeparatorToolsConvertEngineeringDatabase.Visible = false;

                m_MenuItemToolsDebugMode.Visible = false;
                m_SeparatorToolsDebug.Visible = false;

                if (m_ProjectIdentifierPassedAsParameter.Equals(CommonConstants.ProjectIdBART))
                m_MenuItemConfigurePasswordProtection.Enabled = false;
            }
            else if ((short)security.SecurityLevelCurrent >= (short)security.SecurityLevelBase)
            {
                // -----------
                // Maintenance
                // -----------

                // Clear the Visible/Enabled properties of those menu options, including separators, that are not applicable at the base security level.

                // - [File] -
                m_MenuItemFileSelectDataDictionary.Visible = false;

                // - [Diagnostics] -
                m_MenuItemDiagnosticsInitializeEventLogs.Enabled = false;

                // - [Configure] -
                m_MenuItemRealTimeClock.Visible = false;
                m_SeparatorRealTimeClock.Visible = false;
                m_MenuItemCarId.Visible = false;
                m_SeparatorCarId.Visible = false;
                if (m_ProjectIdentifierPassedAsParameter.Equals(CommonConstants.ProjectIdAPM))
                    m_MenuItemConfigurePasswordProtection.Enabled = true;
                else
                m_MenuItemConfigurePasswordProtection.Enabled = false;

                // - [Tools] -
                m_MenuItemTools.Visible = false;
            }
            else
            {
                throw new ArgumentOutOfRangeException("security.SecurityLevelCurrent", "MdiPTU.Support.UpdateMenu()");
            }

            // Call the method to update the form specific changes to the main menu on any of the child forms that are open.
            int mdiChildren = MdiChildren.Length;
            if (MdiChildren.Length > 0)
            {
                for (int childIndex = 0; childIndex < mdiChildren; childIndex++)
                {
                    (MdiChildren[childIndex] as FormPTU).UpdateMenu(m_Security);
                }
            }
            #endregion - [Security] -

            OnMenuUpdated(this, new EventArgs());
        }
        #endregion - [Menu Setup] -

        #region - [Event Management] -
        /// <summary>
        /// Raises a <c>MenuUpdated</c> event. This event is raised when the menu is updated to reflect a new mode or security level.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event.</param>
        /// <param name="eventArgs">Parameter passed from the object that raised the event.</param>
        private void OnMenuUpdated(object sender, EventArgs eventArgs)
        {
            // Skip, if the Dispose() method has been called.
            if (m_IsDisposed)
            {
                return;
            }

            if (MenuUpdated != null)
            {
                MenuUpdated(sender, eventArgs);
            }
        }
        #endregion - [Event Management] -
    }
}
