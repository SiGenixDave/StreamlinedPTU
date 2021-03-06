﻿#region --- Revision History ---
/*
 * 
 *  This document and its contents are the property of Bombardier Inc. or its subsidiaries and contains confidential, proprietary information.
 *  The reproduction, distribution, utilization or the communication of this document, or any part thereof, without express authorization is strictly
 *  prohibited. Offenders will be held liable for the payment of damages.
 * 
 *  (C) 2010    Bombardier Inc. or its subsidiaries. All rights reserved.
 * 
 *  Solution:   Portable Test Unit
 * 
 *  Project:    SelfTest
 * 
 *  File name:  CommunicationSelfTest.cs
 * 
 *  Revision History
 *  ----------------
 */

/*
 *  Date        Version Author          Comments
 *  05/26/11    1.0     K.McD           1.  First entry into TortoiseSVN.
 *  
 *  08/10/11    1.1     Sean.D          1.  Minor corrections to a number of comments.
 *
 */

#region - [1.2] -
/*
 *  03/22/15    1.2     K.McD           References
 *                                      1.  Upgrade the PTU software to extend the support for the R188 project as defined in purchase order
 *                                          4800010525-CU2/19.03.2015.
 *                                      
 *                                          1.  Implement changes outlined in the email to Mark Smorul on 30th May 2014 – PTUDLL32 modifications 
 *                                              to support both 32 and 64 bit architecture.
 *                                      
 *                                      Modifications
 *                                      1.  Added delegate declarations for all of the VcuCommunication32.dll and VcuCommunication64 methods that
 *                                          are associated with the diagnostic self test sub-system.
 *                                          
 *                                      2.  Added member delegates for each of the delegate declarations.
 *                                          
 *                                      3.  Modified the zero parameter constructor to instantiate the delegates with either the 
 *                                          32 or 64 bit version of the corresponding method depending upon the current state of the 
 *                                          'Environment.Is64BitOperatingSystem' variable.
 *                                          
 *                                      4.  Modified each of the methods to check that the function delegate has been initialized prior to its use.
 *                                          
 *                                      5.  Where a method uses a 'Mutex'to ensure that communication with the target hardware is not interrupted, 
 *                                          the method was modified to a check that the Mutex has been initialized prior to its use.
 *                                          
 *                                      6.  Replaced all calls to the methods within PTUDLL32.dll with calls via function delegates. This allows 
 *                                          support for both 32 and 64 bit systems.
 *                                          
 *                                      7. Where a method uses a 'Mutex'to ensure that communication with the target hardware is not interrupted, 
 *                                          a 'finally' block was added to each 'try' block to ensure that the Mutex is released even if an exception 
 *                                          occurs. The code pattern was modified to use the following template:
 *                                          
 *                                          CommunicationError errorCode = CommunicationError.UnknownError;
 *                                          try
 *                                          {
 *                                              m_MutexCommuncationInterface.WaitOne(DefaultMutexWaitDurationMs, false);
 *                                              errorCode = (CommunicationError)m_<function-name>( ... );
 *                                          }
 *                                          catch (Exception)
 *                                          {
 *                                              errorCode = CommunicationError.SystemException;
 *                                              throw new CommunicationException(Resources.EMGetTargetConfigurationFailed, errorCode);
 *                                          }
 *                                          finally
 *                                          {
 *                                              m_MutexCommuncationInterface.ReleaseMutex();
 *                                          }
 *                                          
 *                                          if (DebugMode.Enabled == true)
 *                                          {
 *                                              ...
 *                                          }
 *
 *                                          if (errorCode != CommunicationError.Success)
 *                                          {
 *                                              throw new CommunicationException("<function-name>", errorCode);
 *                                          }
 *                                          
 *                                      8.  Added the unsafe keyword to the delegate declaration for the GetSelfTestResultDelegate() method as
 *                                          the '(byte*) results' parameter can only be used in an unsafe environment. Also modified the
 *                                          GetSelfTestResult() method to make the call via the function delegate within a section of code marked
 *                                          as unsafe.
 *                                          
 *                                      9.  Corrected the name of the StartSelfTestTask() method.
 */
#endregion - [1.2] -

#region - [1.3] -
/*
 *  02/03/2016  1.3     DAS             References
 *                                      1.  Purchase order to replace the unmanaged communication project/dll - VcuCommunication, with the managed communication
 *                                          project/dll - VcuComm1 so that the PTU application is x32/x64 independent.
 *                                      
 *                                      Modifications
 *                                      1.  Replaced all calls to unmanaged code with calls to managed code since the C++ communication DLL is no longer used.
 *                                      2.  Removed all function delegates to the unmanaged SelfTest communication functions along with their associated member variables.
 *                                      3.  Removed the delegate initializations from the constructor. There is now no need to initialize the delegates to either the 32
 *                                          bit or 64 bit version of the event communication methods as managed code supports both 32 bit and 64 bit.
 *                                          architecture.
 */
#endregion - [1.3] -

#region - [1.4] -
/*
 *  02/12/2017  1.4.1   D.Smail         Modifications
 *                                      1.  Add method CommunicationWatchdog() to support watchdog checks with the VCU during self test
 *                                      2.  Remove all references in method headers to PTUDLL32.
 *                                      
 *  02/15/2017  1.4.2   D.Smail         Modifications
 *                                      1.  Added a reference parameter to CommunicationWatchdog() method that is updated to indicate
 *                                          whether or not the target hardware is in self test.
 *                                      
 * 
 */
#endregion - [1.4] -
#endregion --- Revision History ---

using System;
using System.Diagnostics;
using Common;
using Common.Communication;
using Common.Configuration;
using VcuComm;

namespace SelfTest.Communication
{
    /// <summary>
    /// Class to manage the communication with the target hardware with respect to the diagnostic self test sub-system commands.
    /// </summary>
    public class CommunicationSelfTest : CommunicationParent, ICommunicationSelfTest
    {

        /// <summary>
        /// Object that is used to call methods that gather or send information pertaining to self
        /// test execution on the embedded PTU target.
        /// </summary>
        private SelfTestMarshal m_SelfTestMarshal;

        #region --- Constructors ---
        /// <summary>
        /// Initialize a new instance of the class
        /// </summary>
        /// <param name="communicationInterface">Reference to the communication interface containing the properties and member variables that are to be
        /// used to initialize the class.</param>
        public CommunicationSelfTest(ICommunicationParent communicationInterface)
            : base(communicationInterface)
        {
            m_SelfTestMarshal = new SelfTestMarshal(communicationInterface.CommDevice);
        }
        #endregion --- Constructors ---

        #region --- Methods ---
        /// <summary>
        /// Get the self test special message.
        /// </summary>
        /// <param name="result">The result of the call. A value of: (1) 1 represents success; (2) indicates that the error message defined by the 
        /// <paramref name="reason"/> parameter applies and (3) represents an unknown error.</param>
        /// <param name="reason">A value of 1 represents success; otherwise, the value is mapped to the <c>ERRID</c> field of the
        /// <c>SELFTESTERRMESS</c> table 
        /// of the data dictionary in order to determine the error message returned from the VCU.</param>
        /// <exception cref="CommunicationException">Thrown if the error code returned from the call to the m_SelfTestMarshal.GetSelfTestSpecialMessage()
        /// method is not CommunicationError.Success.</exception>
        public void GetSelfTestSpecialMessage(out short result, out short reason)
        {
            // Check that the function delegate has been initialized.
            Debug.Assert(m_MutexCommuncationInterface != null,
                         "CommunicationSelfTest.GetSelfTestSpecialMessage() - [m_MutexCommuncationInterface != null]");

            CommunicationError errorCode = CommunicationError.UnknownError;
            try
            {
                m_MutexCommuncationInterface.WaitOne(DefaultMutexWaitDurationMs, false);
                errorCode = m_SelfTestMarshal.GetSelfTestSpecialMessage(out result, out reason);
            }
            catch (Exception)
            {
                errorCode = CommunicationError.SystemException;
                throw new CommunicationException("CommunicationSelfTest.GetSelfTestSpecialMessage()", errorCode);
            }
            finally
            {
                m_MutexCommuncationInterface.ReleaseMutex();
            }

            if (DebugMode.Enabled == true)
            {
                DebugMode.GetSelfTestSpecialMessage_t getSelfTestSpecialMessage = new DebugMode.GetSelfTestSpecialMessage_t(result, reason, errorCode);
                DebugMode.Write(getSelfTestSpecialMessage.ToXML());
            }

            if (errorCode != CommunicationError.Success)
            {
                throw new CommunicationException("CommunicationSelfTest.GetSelfTestSpecialMessage()", errorCode);
            }
        }

        /// <summary>
        /// Start the self test task.
        /// </summary>
        /// <remarks>This request will start the self test process on the VCU.</remarks>
        /// <param name="result">The result of the call. A value of: (1) 1 represents success; (2) indicates that the error message defined by the 
        /// <paramref name="reason"/> parameter applies and (3) represents an unknown error.</param>
        /// <param name="reason">A value of 1 represents success; otherwise, the value is mapped to the <c>ERRID</c> field of the
        /// <c>SELFTESTERRMESS</c> table 
        /// of the data dictionary in order to determine the error message returned from the VCU.</param>
        /// <exception cref="CommunicationException">Thrown if the error code returned from the call to the m_SelfTestMarshal.StartSelfestTask() method is not 
        /// CommunicationError.Success.</exception>
        public void StartSelfTestTask(out short result, out short reason)
        {

            Debug.Assert(m_MutexCommuncationInterface != null,
                         "CommunicationSelfTest.StartSelfTestTask() - [m_MutexCommuncationInterface != null]");

            CommunicationError errorCode = CommunicationError.UnknownError;
            try
            {
                m_MutexCommuncationInterface.WaitOne(DefaultMutexWaitDurationMs, false);
                errorCode = m_SelfTestMarshal.StartSelfTestTask(out result, out reason);
            }
            catch (Exception)
            {
                errorCode = CommunicationError.SystemException;
                throw new CommunicationException("CommunicationSelfTest.StartSelfTestTask()", errorCode);
            }
            finally
            {
                m_MutexCommuncationInterface.ReleaseMutex();
            }

            if (DebugMode.Enabled == true)
            {
                DebugMode.StartSelfTestTask_t startSelfTestTask = new DebugMode.StartSelfTestTask_t(result, reason, errorCode);
                DebugMode.Write(startSelfTestTask.ToXML());
            }

            if (errorCode != CommunicationError.Success)
            {
                throw new CommunicationException("CommunicationSelfTest.StartSelfTestTask()", errorCode);
            }
        }

        /// <summary>
        /// Exit the self test task.
        /// </summary>
        /// <remarks>This request will end the self test process on the VCU.</remarks>
        /// <param name="result">The result of the call. A value of: (1) 1 represents success; (2) indicates that the error message defined by the 
        /// <paramref name="reason"/> parameter applies and (3) represents an unknown error.</param>
        /// <param name="reason">A value of 1 represents success; otherwise, the value is mapped to the <c>ERRID</c> field of the
        /// <c>SELFTESTERRMESS</c> table 
        /// of the data dictionary in order to determine the error message returned from the VCU.</param>
        /// <exception cref="CommunicationException">Thrown if the error code returned from the call to the m_SelfTestMarshal.ExitSelfestTask() method is not 
        /// CommunicationError.Success.</exception>
        public void ExitSelfTestTask(out short result, out short reason)
        {
            Debug.Assert(m_MutexCommuncationInterface != null,
                         "CommunicationSelfTest.ExitSelfTestTask() - [m_MutexCommuncationInterface != null]");

            CommunicationError errorCode = CommunicationError.UnknownError;
            try
            {
                m_MutexCommuncationInterface.WaitOne(DefaultMutexWaitDurationMs, false);
                errorCode = m_SelfTestMarshal.ExitSelfTestTask(out result, out reason);
            }
            catch (Exception)
            {
                errorCode = CommunicationError.SystemException;
                throw new CommunicationException("CommunicationSelfTest.ExitSelfTestTask()", errorCode);
            }
            finally
            {
                m_MutexCommuncationInterface.ReleaseMutex();
            }

            if (DebugMode.Enabled == true)
            {
                DebugMode.ExitSelfTestTask_t exitSelfTestTask = new DebugMode.ExitSelfTestTask_t(result, reason, errorCode);
                DebugMode.Write(exitSelfTestTask.ToXML());
            }

            if (errorCode != CommunicationError.Success)
            {
                throw new CommunicationException("CommunicationSelfTest.ExitSelfestTask()", errorCode);
            }
        }

        /// <summary>
        /// Abort the self test sequence.
        /// </summary>
        /// <remarks>This request will stop the execution of the self-test process on the VCU and return control to the propulsion software.</remarks>
        /// <exception cref="CommunicationException">Thrown if the error code returned from the call to the m_SelfTestMarshal.AbortSTSequence() method is not 
        /// CommunicationError.Success.</exception>
        public void AbortSTSequence()
        {
            Debug.Assert(m_MutexCommuncationInterface != null,
                         "CommunicationSelfTest.AbortSTSequence() - [m_MutexCommuncationInterface != null]");

            CommunicationError errorCode = CommunicationError.UnknownError;
            try
            {
                m_MutexCommuncationInterface.WaitOne(DefaultMutexWaitDurationMs, false);
                errorCode = m_SelfTestMarshal.AbortSTSequence();
            }
            catch (Exception)
            {
                errorCode = CommunicationError.SystemException;
                throw new CommunicationException("CommunicationSelfTest.AbortSTSequence()", errorCode);
            }
            finally
            {
                m_MutexCommuncationInterface.ReleaseMutex();
            }

            if (DebugMode.Enabled == true)
            {
                DebugMode.AbortSTSequence_t abortSTSequence = new DebugMode.AbortSTSequence_t(errorCode);
                DebugMode.Write(abortSTSequence.ToXML());
            }

            if (errorCode != CommunicationError.Success)
            {
                throw new CommunicationException("CommunicationSelfTest.AbortSTSequence()", errorCode);
            }
        }

        /// <summary>
        /// Send an operator acknowledge message.
        /// </summary>
        /// <remarks>This request allows the operator to move to the next step of an interactive test.</remarks>
        /// <exception cref="CommunicationException">Thrown if the error code returned from the call to the m_SelfTestMarshal.SendOperatorAcknowledge() method
        /// is not CommunicationError.Success.</exception>
        public void SendOperatorAcknowledge()
        {
            Debug.Assert(m_MutexCommuncationInterface != null,
                         "CommunicationSelfTest.SendOperatorAcknowledge() - [m_MutexCommuncationInterface != null]");

            CommunicationError errorCode = CommunicationError.UnknownError;
            try
            {
                m_MutexCommuncationInterface.WaitOne(DefaultMutexWaitDurationMs, false);
                errorCode = m_SelfTestMarshal.SendOperatorAcknowledge();
            }
            catch (Exception)
            {
                errorCode = CommunicationError.SystemException;
                throw new CommunicationException("CommunicationSelfTest.SendOperatorAcknowledge()", errorCode);
            }
            finally
            {
                m_MutexCommuncationInterface.ReleaseMutex();
            }

            if (DebugMode.Enabled == true)
            {
                DebugMode.SendOperatorAcknowledge_t sendOperatorAcknowledge = new DebugMode.SendOperatorAcknowledge_t(errorCode);
                DebugMode.Write(sendOperatorAcknowledge.ToXML());
            }

            if (errorCode != CommunicationError.Success)
            {
                throw new CommunicationException("CommunicationSelfTest.SendOperatorAcknowledge()", errorCode);
            }
        }

        /// <summary>
        /// Send self test communication watchdog message to the VCU 
        /// </summary>
        /// <remarks>This method will send a communication watchdog message to the VCU. A response is expected back from the VCU. No action
        /// is taken in the VCU side. </remarks>
        /// <exception cref="CommunicationException">Thrown if the error code returned from the call to m_SelfTestMarshal.CommunicationWatchdog() method is not 
        /// CommunicationError.Success.</exception>
        /// <param name="InSelfTest">true if target hardware is in self test mode; false otherwise</param>
        public void CommunicationWatchdog(ref Boolean InSelfTest)
        {

            Debug.Assert(m_MutexCommuncationInterface != null,
                         "CommunicationSelfTest.CommunicationWatchdog() - [m_MutexCommuncationInterface != null]");

            CommunicationError errorCode = CommunicationError.UnknownError;
            try
            {
                m_MutexCommuncationInterface.WaitOne(DefaultMutexWaitDurationMs, false);
                errorCode = m_SelfTestMarshal.CommunicationWatchdog(ref InSelfTest);
            }
            catch (Exception)
            {
                errorCode = CommunicationError.SystemException;
                throw new CommunicationException("CommunicationSelfTest.CommunicationWatchdog()", errorCode);
            }
            finally
            {
                m_MutexCommuncationInterface.ReleaseMutex();
            }

            if (DebugMode.Enabled == true)
            {
                //TODO DAS DebugMode.UpdateSTTestList_t updateSTTestList = new DebugMode.UpdateSTTestList_t(testCount, tests, errorCode);
                //TODO DAS DebugMode.Write(updateSTTestList.ToXML());
            }

            if (errorCode != CommunicationError.Success)
            {
                throw new CommunicationException("CommunicationSelfTest.CommunicationWatchdog()", errorCode);
            }
        }




        /// <summary>
        /// Update the list of individually selected self tests that are to be executed. 
        /// </summary>
        /// <remarks>This method will define the list of self-tests that are to be executed once the tester selects the execute command. The self tests
        /// are defined using the self test identifiers defined in the data dictionary.</remarks>
        /// <param name="testCount">The number of tests in the list.</param>
        /// <param name="tests">A list of the selfTestIdentifiers.</param>
        /// <exception cref="CommunicationException">Thrown if the error code returned from the call to the m_SelfTestMarshal.UpdateSTTestList() method is not 
        /// CommunicationError.Success.</exception>
        public void UpdateSTTestList(short testCount, short[] tests)
        {
            Debug.Assert(m_MutexCommuncationInterface != null,
                         "CommunicationSelfTest.UpdateSTTestList() - [m_MutexCommuncationInterface != null]");

            CommunicationError errorCode = CommunicationError.UnknownError;
            try
            {
                m_MutexCommuncationInterface.WaitOne(DefaultMutexWaitDurationMs, false);
                errorCode = m_SelfTestMarshal.UpdateSTTestList(testCount, tests);
            }
            catch (Exception)
            {
                errorCode = CommunicationError.SystemException;
                throw new CommunicationException("CommunicationSelfTest.UpdateSTTestList()", errorCode);
            }
            finally
            {
                m_MutexCommuncationInterface.ReleaseMutex();
            }

            if (DebugMode.Enabled == true)
            {
                DebugMode.UpdateSTTestList_t updateSTTestList = new DebugMode.UpdateSTTestList_t(testCount, tests, errorCode);
                DebugMode.Write(updateSTTestList.ToXML());
            }

            if (errorCode != CommunicationError.Success)
            {
                throw new CommunicationException("CommunicationSelfTest.UpdateSTTestList()", errorCode);
            }
        }

        /// <summary>
        /// Run the predefined self tests associated with the specified test list identifier, these tests are defined in the data dictionary. 
        /// </summary>
        /// <param name="testListIdentifier">The test list identifier of the predefined self tests that are to be executed.</param>
        /// <exception cref="CommunicationException">Thrown if the error code returned from the call to the m_SelfTestMarshal.RunPredefinedSTTests() method is
        /// not CommunicationError.Success.</exception>
        public void RunPredefinedSTTests(short testListIdentifier)
        {
            Debug.Assert(m_MutexCommuncationInterface != null,
                         "CommunicationSelfTest.RunPredefinedSTTests() - [m_MutexCommuncationInterface != null]");

            CommunicationError errorCode = CommunicationError.UnknownError;
            try
            {
                m_MutexCommuncationInterface.WaitOne(DefaultMutexWaitDurationMs, false);
                errorCode = m_SelfTestMarshal.RunPredefinedSTTests(testListIdentifier);
            }
            catch (Exception)
            {
                errorCode = CommunicationError.SystemException;
                throw new CommunicationException("CommunicationSelfTest.RunPredefinedSTTests()", errorCode);
            }
            finally
            {
                m_MutexCommuncationInterface.ReleaseMutex();
            }

            if (DebugMode.Enabled == true)
            {
                DebugMode.RunPredefinedSTTests_t runPredefinedSTTests = new DebugMode.RunPredefinedSTTests_t(testListIdentifier, errorCode);
                DebugMode.Write(runPredefinedSTTests.ToXML());
            }

            if (errorCode != CommunicationError.Success)
            {
                throw new CommunicationException("CommunicationSelfTest.RunPredefinedSTTests()", errorCode);
            }
        }

        /// <summary>
        /// Update the number of times that the selected tests are to be run.
        /// </summary>
        /// <param name="loopCount">The number of cycles/loops of the defined tests that are to be performed.</param>
        /// <exception cref="CommunicationException">Thrown if the error code returned from the call to the m_SelfTestMarshal.UpdateSTLoopCount() method is not 
        /// CommunicationError.Success.</exception>
        public void UpdateSTLoopCount(short loopCount)
        {
            // Check that the function delegate has been initialized.
            Debug.Assert(m_MutexCommuncationInterface != null,
                         "CommunicationSelfTest.UpdateSTLoopCount() - [m_MutexCommuncationInterface != null]");

            CommunicationError errorCode = CommunicationError.UnknownError;
            try
            {
                m_MutexCommuncationInterface.WaitOne(DefaultMutexWaitDurationMs, false);
                errorCode = m_SelfTestMarshal.UpdateSTLoopCount(loopCount);
            }
            catch (Exception)
            {
                errorCode = CommunicationError.SystemException;
                throw new CommunicationException("CommunicationSelfTest.UpdateSTLoopCount()", errorCode);
            }
            finally
            {
                m_MutexCommuncationInterface.ReleaseMutex();
            }

            if (DebugMode.Enabled == true)
            {
                DebugMode.UpdateSTLoopCount_t updateSTLoopCount = new DebugMode.UpdateSTLoopCount_t(loopCount, errorCode);
                DebugMode.Write(updateSTLoopCount.ToXML());
            }

            if (errorCode != CommunicationError.Success)
            {
                throw new CommunicationException("CommunicationSelfTest.UpdateSTLoopCount()", errorCode);
            }
        }

        /// <summary>
        /// Execute the self tests that are defined in the current list.
        /// </summary>
        /// <param name="truckInformation">The truck to which the self tests apply. This does not apply on the CTA project as separate self-tests are
        /// set up for each truck.</param>
        /// <exception cref="CommunicationException">Thrown if the error code returned from the call to the m_SelfTestMarshal.ExecuteSTTestList() method is not
        /// CommunicationError.Success.</exception>
        public void ExecuteSTTestList(TruckInformation truckInformation)
        {
            Debug.Assert(m_MutexCommuncationInterface != null,
                         "CommunicationSelfTest.ExecuteSTTestList() - [m_MutexCommuncationInterface != null]");

            CommunicationError errorCode = CommunicationError.UnknownError;
            try
            {
                m_MutexCommuncationInterface.WaitOne(DefaultMutexWaitDurationMs, false);
                errorCode = m_SelfTestMarshal.ExecuteSTTestList((short)truckInformation);
            }
            catch (Exception)
            {
                errorCode = CommunicationError.SystemException;
                throw new CommunicationException("CommunicationSelfTest.ExecuteSTTestList()", errorCode);
            }
            finally
            {
                m_MutexCommuncationInterface.ReleaseMutex();
            }

            if (DebugMode.Enabled == true)
            {
                DebugMode.ExecuteSTTestList_t executeSTTestList = new DebugMode.ExecuteSTTestList_t(truckInformation, errorCode);
                DebugMode.Write(executeSTTestList.ToXML());
            }

            if (errorCode != CommunicationError.Success)
            {
                throw new CommunicationException("CommunicationSelfTest.ExecuteSTTestList()", errorCode);
            }
        }

        /// <summary>
        /// Get the self test results.
        /// </summary>
        /// <param name="resultAvailable">A flag to indicate whether a valid result is available. A value of 1 indicates that a valid result is
        /// available; otherwise,  0.</param>
        /// <param name="messageMode">The type of message returned from the VCU.</param>
        /// <param name="testIdentifier">The test result identifier; the interpretation of this value is dependent upon the message mode. For detailed
        /// messages, this value represents the self test identifier.</param>
        /// <param name="testCase">The test case number associated with the message.</param>
        /// <param name="testResult">Used with the passive and logic self tests to define whether the test passed or failed. A value of 1 indicates
        /// that the test passed; otherwise, the test failed.</param>
        /// <param name="truckInformation">An enumerator to define the truck information associated with the message.</param>
        /// <param name="variableCount">The number of variables associated with the message.</param>
        /// <param name="results">An array of <see cref="InteractiveResults_t"/> structures containing the value of each self test variable associated
        /// with the current interactive test.</param>
        /// <exception cref="CommunicationException">Thrown if the error code returned from the call to the m_SelfTestMarshal.GetSelfTestResult() method is not 
        /// CommunicationError.Success.</exception>
        /// <remarks>In C# the sizeof the InteractiveResults_t structure is 16 bytes as the size is rounded up to the nearest quad word. This is 
        /// inconsistent with the size of the InteractiveResults_t structure - 12 bytes. To ensure that the results are 
        /// interpreted correctly the results are passed as a byte array which is then mapped to an array of InteractiveResults structures.</remarks>
        public void GetSelfTestResult(out short resultAvailable, out MessageMode messageMode, out short testIdentifier, out short testCase,
                                             out short testResult, out TruckInformation truckInformation, out short variableCount,
                                             out InteractiveResults_t[] results)
        {

            Debug.Assert(m_MutexCommuncationInterface != null,
                         "CommunicationSelfTest.GetSelfTestResult() - [m_MutexCommuncationInterface != null]");

            results = new InteractiveResults_t[Parameter.WatchSizeInteractiveTest];

            CommunicationError errorCode = CommunicationError.UnknownError;
            try
            {
                m_MutexCommuncationInterface.WaitOne(DefaultMutexWaitDurationMs, false);
                errorCode = m_SelfTestMarshal.GetSelfTestResult(out resultAvailable, out messageMode, out testIdentifier, out testCase, out testResult,
                                                                out truckInformation, out variableCount, results);
            }
            catch (Exception)
            {
                errorCode = CommunicationError.SystemException;
                throw new CommunicationException("CommunicationSelfTest.GetSelfTestResult()", errorCode);
            }
            finally
            {
                m_MutexCommuncationInterface.ReleaseMutex();
            }


            if (DebugMode.Enabled == true)
            {
                DebugMode.GetSelfTestResult_t getSelfTestResult = new DebugMode.GetSelfTestResult_t(resultAvailable, messageMode, testIdentifier,
                                                                                                    testCase, testResult, truckInformation,
                                                                                                    variableCount, results, errorCode);
                DebugMode.Write(getSelfTestResult.ToXML());
            }

            if (errorCode != CommunicationError.Success)
            {
                throw new CommunicationException("CommunicationSelfTest.GetSelfTestResult()", errorCode);
            }
        }

        /// <summary>
        /// Update the self test mode.
        /// </summary>
        /// <remarks>This call is used to check whether communication with the VCU has been lost.</remarks>
        /// <param name="selfTestMode">The required self test mode.</param>
        /// <exception cref="CommunicationException">Thrown if the error code returned from the call to the m_SelfTestMarshal.UpdateSTMode() method is not 
        /// CommunicationError.Success.</exception>
        public void UpdateSTMode(SelfTestMode selfTestMode)
        {
            Debug.Assert(m_MutexCommuncationInterface != null,
                         "CommunicationSelfTest.UpdateSTMode() - [m_MutexCommuncationInterface != null]");

            CommunicationError errorCode = CommunicationError.UnknownError;
            try
            {
                m_MutexCommuncationInterface.WaitOne(DefaultMutexWaitDurationMs, false);
                errorCode = m_SelfTestMarshal.UpdateSTMode((Int16)selfTestMode);
            }
            catch (Exception)
            {
                errorCode = CommunicationError.SystemException;
                throw new CommunicationException("CommunicationSelfTest.UpdateSTMode()", errorCode);
            }
            finally
            {
                m_MutexCommuncationInterface.ReleaseMutex();
            }

            if (DebugMode.Enabled == true)
            {
                DebugMode.UpdateSTMode_t updateSTMode = new DebugMode.UpdateSTMode_t(selfTestMode, errorCode);
                DebugMode.Write(updateSTMode.ToXML());
            }

            if (errorCode != CommunicationError.Success)
            {
                throw new CommunicationException("CommunicationSelfTest.UpdateSTMode()", errorCode);
            }
        }
        #endregion --- Methods ---
    }
}
