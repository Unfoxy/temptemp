﻿using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.IO;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.ServiceProcess;
using System.Management;
using Microsoft.Win32;
using System.Windows.Forms.VisualStyles;

namespace desktopDashboard___Y_Lee
{
    public class Functions
    {
        public static Tuple<string[], string[], int> queryAD(string site, string filter)
        {
            DirectoryEntry ldapConnection = new DirectoryEntry("");
            ldapConnection.Path = "LDAP://OU=Client,DC=in1,DC=ad,DC=innovene,DC=com"; //Default All Sites
            ldapConnection.AuthenticationType = AuthenticationTypes.Secure;
            DirectorySearcher search = new DirectorySearcher(ldapConnection);
            var expiredTime = DateTime.Now.AddDays(-180).ToFileTime();                //Account expiration date 180days

            if (site.ToUpper() == "BMC")
            {
                ldapConnection.Path = "LDAP://OU=BMC,OU=rAM,OU=Client,DC=in1,DC=ad,DC=innovene,DC=com";
            }
            else if (site.ToUpper() == "MVW")
            {
                ldapConnection.Path = "LDAP://OU=MVW,OU=rAM,OU=Client,DC=in1,DC=ad,DC=innovene,DC=com";
            }
            else if (site.ToUpper() == "CHO")
            {
                ldapConnection.Path = "LDAP://OU=CHO,OU=rAM,OU=Client,DC=in1,DC=ad,DC=innovene,DC=com";
            }
            else if (site.ToUpper() == "LAR")
            {
                ldapConnection.Path = "LDAP://OU=LAR,OU=rAM,OU=Client,DC=in1,DC=ad,DC=innovene,DC=com";
            }
            else if (site.ToUpper() == "HDC")
            {
                ldapConnection.Path = "LDAP://OU=HDC,OU=rAM,OU=Client,DC=in1,DC=ad,DC=innovene,DC=com";
            }

            if (filter == "Account Deactivated Users")
            {
                search.Filter = "(&"
                                    + "(userAccountControl=" + "514" + ")"
                                    + "(extensionAttribute2=" + "OP USA" + ")"
                                    //+ "(extensionAttribute12=" + "OPUSA_O365" + ")"   //AT12 filters few contractors
                                    + ")";
            }
            else if (filter == "Password Expired Users")
            {
                search.Filter = "(&"
                                    + "(pwdLastSet<=" + expiredTime + ")"
                                    + "(extensionAttribute2=" + "OP USA" + ")"
                                    //+ "(extensionAttribute12=" + "OPUSA_O365" + ")"
                                    + "(userAccountControl=" + "512" + ")"
                                    + ")";
            }
            else if (filter == "Account Locked Users")
            {
                search.Filter = "(&"
                                    + "(lockoutTime>=" + "1" + ")"
                                    + "(extensionAttribute2=" + "OP USA" + ")"
                                    //+ "(extensionAttribute12=" + "OPUSA_O365" + ")"
                                    + ")";
            }
            else if (filter == "Account Expired Users")
            {
                search.Filter = "(&"
                                   + "(accountExpires<=" + expiredTime + ")"
                                   + "(userAccountControl=" + "512" + ")"
                                   //+ "(extensionAttribute12=" + "OPUSA_O365" + ")"
                                   + "(extensionAttribute2=" + "OP USA" + ")"
                                   + "(!" + "(accountExpires=" + "0" + ")"
                                   + ")"
                                   + ")";
            }

            try
            {
                string[] requiredProperties = new string[] { "cn", "sAMAccountName" };

                foreach (String property in requiredProperties)
                    search.PropertiesToLoad.Add(property);

                SearchResultCollection result = search.FindAll();

                string[] name = new string[1000];       //Max number for the output
                string[] ntid = new string[1000];       //Max number for the output
                int count = 0;
                foreach (SearchResult userResults in result)
                {
                    name[count] = userResults.Properties["cn"][0].ToString();
                    ntid[count] = userResults.Properties["sAMAccountName"][0].ToString();
                    count++;
                }

                return Tuple.Create(name, ntid, count);
            }
            catch
            {
                throw;
            }
        }
        public static int userCount(string site, string filter)
        {
            DirectoryEntry ldapConnection = new DirectoryEntry("");
            ldapConnection.Path = "LDAP://OU=Client,DC=in1,DC=ad,DC=innovene,DC=com"; //Default All Sites
            ldapConnection.AuthenticationType = AuthenticationTypes.Secure;
            DirectorySearcher search = new DirectorySearcher(ldapConnection);
            var expiredTime = DateTime.Now.AddDays(-180).ToFileTime();                //Account expiration date

            if (site.ToUpper() == "BMC:")
            {
                ldapConnection.Path = "LDAP://OU=BMC,OU=rAM,OU=Client,DC=in1,DC=ad,DC=innovene,DC=com";
            }
            else if (site.ToUpper() == "MVW:")
            {
                ldapConnection.Path = "LDAP://OU=MVW,OU=rAM,OU=Client,DC=in1,DC=ad,DC=innovene,DC=com";
            }
            else if (site.ToUpper() == "CHO:")
            {
                ldapConnection.Path = "LDAP://OU=CHO,OU=rAM,OU=Client,DC=in1,DC=ad,DC=innovene,DC=com";
            }
            else if (site.ToUpper() == "LAR:")
            {
                ldapConnection.Path = "LDAP://OU=LAR,OU=rAM,OU=Client,DC=in1,DC=ad,DC=innovene,DC=com";
            }
            else if (site.ToUpper() == "HDC:")
            {
                ldapConnection.Path = "LDAP://OU=HDC,OU=rAM,OU=Client,DC=in1,DC=ad,DC=innovene,DC=com";
            }

            if (filter == "Account Deactivated Users")
            {
                search.Filter = "(&"
                                    + "(userAccountControl=" + "514" + ")"
                                    + "(extensionAttribute2=" + "OP USA" + ")"
                                    //+ "(extensionAttribute12=" + "OPUSA_O365" + ")"   //AT12 filters few contractors
                                    + ")";
            }
            else if (filter == "Password Expired Users")
            {
                search.Filter = "(&"
                                    + "(pwdLastSet<=" + expiredTime + ")"
                                    + "(extensionAttribute2=" + "OP USA" + ")"
                                    //+ "(extensionAttribute12=" + "OPUSA_O365" + ")"
                                    + "(userAccountControl=" + "512" + ")"
                                    + ")";
            }
            else if (filter == "Account Locked Users")
            {
                search.Filter = "(&"
                                    + "(lockoutTime>=" + "1" + ")"
                                    + "(extensionAttribute2=" + "OP USA" + ")"
                                    //+ "(extensionAttribute12=" + "OPUSA_O365" + ")"
                                    + ")";
            }
            else if (filter == "Account Expired Users")
            {
                search.Filter = "(&"
                                    + "(accountExpires<=" + expiredTime + ")"
                                    + "(userAccountControl=" + "512" + ")"
                                    + "(extensionAttribute2=" + "OP USA" + ")"
                                    //+ "(extensionAttribute12=" + "OPUSA_O365" + ")"
                                    + "(!" + "(accountExpires=" + "0" + ")"
                                    + ")"
                                    + ")";
            }

            try
            {
                SearchResultCollection result = search.FindAll();
                return result.Count;
            }
            catch
            {
                throw;
            }
        }
        public static void editRegistry(string hostname)
        {
            var inputRegistry = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, hostname);
            inputRegistry = inputRegistry.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}\0001", true); //IPChecksumOffloadIPv4 location
            inputRegistry.SetValue("*IPChecksumOffloadIPv4", "0");
        }
        public static void startupManual(string hostname)
        {
            ManagementBaseObject inParam;
            ManagementBaseObject outParam;
            var serviceController = new ServiceController();
            ManagementObject obj = new ManagementObject(@"\\" + hostname + "\\root\\cimv2:Win32_Service.Name='RemoteRegistry'");
            try
            {
                if (obj["StartMode"].ToString() == "Disabled")
                {
                    inParam = obj.GetMethodParameters("ChangeStartMode");
                    inParam["StartMode"] = "Manual";
                    outParam = obj.InvokeMethod("ChangeStartMode", inParam, null);
                }
            }
            catch
            {
                throw;
            }
        }
        public static void startupDisabled(string hostname)
        {
            ManagementBaseObject inParam;
            ManagementBaseObject outParam;
            var serviceController = new ServiceController();
            ManagementObject obj = new ManagementObject(@"\\" + hostname + "\\root\\cimv2:Win32_Service.Name='RemoteRegistry'");
            try
            {
                if (obj["StartMode"].ToString() == "Manual")
                {
                    inParam = obj.GetMethodParameters("ChangeStartMode");
                    inParam["StartMode"] = "Disabled";
                    outParam = obj.InvokeMethod("ChangeStartMode", inParam, null);
                }
            }
            catch
            {
                throw;
            }
        }
        public static void startStopService(string hostname)
        {
            try
            {
                string serviceName = "RemoteRegistry";

                ServiceController serviceController = new ServiceController("Remote Registry", hostname);
                ConnectionOptions connectoptions = new ConnectionOptions();
                ManagementScope scope = new ManagementScope("\\\\" + hostname + "\\root\\CIMV2");
                scope.Options = connectoptions;
                
                SelectQuery query = new SelectQuery("select * from Win32_Service where name = '" + serviceName + "'");          //WMI query to be executed on the remote machine  
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
                {
                    ManagementObjectCollection collection = searcher.Get();
                    foreach (ManagementObject service in collection.OfType<ManagementObject>())
                    {
                        if (service["started"].Equals(true))
                        {
                            service.InvokeMethod("StopService", null);
                            serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                        }
                        else
                        {
                            service.InvokeMethod("StartService", null);
                            serviceController.WaitForStatus(ServiceControllerStatus.Running);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        public static void resetPassword(string username)
        {
            PrincipalContext context = new PrincipalContext(ContextType.Domain);
            UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, username);
            //user.Enabled = true;            //Enable Account if it is disabled - Not Working right now
            user.SetPassword("Ineos2023");
            user.ExpirePasswordNow();         //Force user to change password at next logon
            user.Save();
        }
        public static string[] GetAD(string username)
        {
            try
            {
                DirectoryEntry ldapConnection = new DirectoryEntry("");
                ldapConnection.Path = "LDAP://OU=Client,DC=in1,DC=ad,DC=innovene,DC=com";           //For all Ineos sites
                ldapConnection.AuthenticationType = AuthenticationTypes.Secure;
                DirectorySearcher search = new DirectorySearcher(ldapConnection);

                search.Filter = "(|"                                                                //Multiple search options
                                   + "(sAMAccountName=" + username + ")"
                                   + "(mail=" + username + ")"
                                   + "(mail=" + username + "@ineos.com" + ")"
                                   + "(cn=" + username + ")"
                                   + ")";

                string[] requiredProperties = new string[] { "cn",
                                                           "mail",
                                                 "sAMAccountName",
                                     "physicalDeliveryOfficeName",
                                                "telephoneNumber", 
                                                          "title" };

                foreach (String property in requiredProperties)
                    search.PropertiesToLoad.Add(property);

                SearchResult result = search.FindOne();

                if (result != null)
                {
                    string[] results = new string[6] { "", "", "", "", "", "" };
                    int i = 0;
                    foreach (String property in requiredProperties)
                        foreach (Object myCollection in result.Properties[property])
                        {
                            if (results[i] != "")
                                i++;
                            results[i] = myCollection.ToString();
                        }
                    return results;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
        public static string[] pingHostname(string hostname)
        {
            Ping myPing = new Ping();
            string[] pingResults = new string[2] { "", "" };
            string[] failedResult = new string[1] { "TimeOut" };
            try
            {
                PingReply reply = myPing.Send(hostname, 10000);

                if (reply.Status.ToString() == "Success")
                {
                    pingResults[0] = reply.Address.ToString();
                    pingResults[1] = reply.RoundtripTime.ToString();
                    return pingResults;
                }
                else
                    return null;
            }
            catch
            {
                return failedResult;
            }
        }
        public static string[,] copyfiles(string newPc, string oldPc, string username, string item)
        {
            
            if (item == "Edge")
            {
                string[,] result = new string[2,3];
                string[,] edge = copyPath(newPc, oldPc, username, item);
                
                for (int i = 0; i<3; i++)
                {
                    if(!(File.Exists(edge[0, i])))
                    {
                        Array.Clear(edge, i, 1);
                        result[0, i] = (i+1).ToString();            //return numbers when transfer failed
                    }
                    else
                        File.Copy(edge[0, i], edge[1, i], true);    //return Null when transfer success
                }
                return result;
            }
            else
            {
                string[,] result = new string[2, 2];
                string[,] chrome = copyPath(newPc, oldPc, username, item);
                for (int i = 0; i < 2; i++)
                {
                    if (!(File.Exists(chrome[0, i])))
                    {
                        Array.Clear(chrome, i, 1);
                        result[0, i] = (i+1).ToString();                //return numbers when transfer failed
                    }
                    else
                        File.Copy(chrome[0, i], chrome[1, i], true);    //return Null when transfer success
                }
                return result;
            }
        }
        public static bool copyDirectory(string newPc, string oldPc, string username, string item)
        {
            if (item == "Quick Access")
            {
                string[,] quickAccess = copyPath(newPc, oldPc, username, item);
                var files = new DirectoryInfo(quickAccess[1, 0]).GetFiles("*.*");

                foreach (FileInfo file in files)
                {
                    file.CopyTo(quickAccess[0, 0] + file.Name, true);
                }
                return true;
            }
            else if (item == "Outlook Signatures")
            {
                string[,] outlookSignatures = copyPath(newPc, oldPc, username, item);

                if (!Directory.Exists(outlookSignatures[0, 0]))            //Copy directory has to overwrite the exsiting folder
                {
                    Directory.CreateDirectory(outlookSignatures[0, 0]);
                }

                if (!Directory.Exists(outlookSignatures[1, 0]))
                {
                    return false;
                }
                else
                {
                    var files = new DirectoryInfo(outlookSignatures[1, 0]).GetFiles("*.*");
                    foreach (FileInfo file in files)
                    {
                        file.CopyTo(outlookSignatures[0, 0] + file.Name, true);
                    }
                    return true;
                }
            }
            else
                return false;
        }
        public static string[,] confirmPath(string newPc, string oldPc, string username)    //Only to confirm both path are existed
        {                                                                                   //If a user doesn't sign on new PC, Path won't be found
            string[,] path = new string[2, 1]
            {
                { @"\\" + oldPc + @"\c$\users\" + username},
                { @"\\" + newPc + @"\c$\users\" + username}
            };
            return path;
        }
        public static string[,] copyPath(string newPc, string oldPc, string username, string item)
        {
            if (item == "Edge")
            {
                string[,] path = new string[2, 3]
                {
                    { @"\\" + oldPc + @"\c$\users\" + username + @"\appdata\local\microsoft\edge\user data\default\bookmarks",
                        @"\\" + oldPc + @"\c$\users\" + username + @"\appdata\local\microsoft\edge\user data\default\bookmarks.bak",
                        @"\\" + oldPc + @"\c$\users\" + username + @"\appdata\local\microsoft\edge\user data\default\bookmarks.msbak" },
                    { @"\\" + newPc + @"\c$\users\" + username + @"\appdata\local\microsoft\edge\user data\default\bookmarks",
                        @"\\" + newPc + @"\c$\users\" + username + @"\appdata\local\microsoft\edge\user data\default\bookmarks.bak",
                        @"\\" + newPc + @"\c$\users\" + username + @"\appdata\local\microsoft\edge\user data\default\bookmarks.msbak" }
                };
                return path;
            }
            else if (item == "Chrome")
            {
                string[,] path = new string[2, 2]
                {
                    { @"\\" + oldPc + @"\c$\users\" + username + @"\appdata\local\google\chrome\user data\default\bookmarks",
                        @"\\" + oldPc + @"\c$\users\" + username + @"\appdata\local\google\chrome\user data\default\bookmarks.bak",},
                    { @"\\" + newPc + @"\c$\users\" + username + @"\appdata\local\google\chrome\user data\default\bookmarks",
                        @"\\" + newPc + @"\c$\users\" + username + @"\appdata\local\google\chrome\user data\default\bookmarks.bak",}
                };
                return path;
            }
            else if (item == "Quick Access")
            {
                string[,] path = new string[2, 1]
                {
                    { @"\\" + newPc + @"\c$\users\" + username + @"\appdata\roaming\microsoft\Windows\Recent\automaticDestinations\"},
                    {@"\\" + oldPc + @"\c$\users\" + username + @"\appdata\roaming\microsoft\Windows\Recent\automaticDestinations"}
                };
                return path;
            }
            else if (item == "Outlook Signatures")
            {
                string[,] path = new string[2, 1]
                {
                    { @"\\" + newPc + @"\c$\users\" + username + @"\appdata\roaming\microsoft\Signatures\"},
                    {@"\\" + oldPc + @"\c$\users\" + username + @"\appdata\roaming\microsoft\Signatures"}
                };
                return path;
            }
            else
                return null;
        }
        public static void createUser()
        {
            //try
            //{
            //    PrincipalContext pricipalContext = null;
            //    pricipalContext = new PrincipalContext(ContextType.Domain, "in1.ad.innovene.com", "OU=Client,DC=in1,DC=ad,DC=innovene,DC=com");
            //    //Sometimes we need to connect to AD using service/admin account credentials, in that case the above line of code will be as below
            //    //pricipalContext = new PrincipalContext(ContextType.Domain, "yourdomain.com", "OU=TestOU,DC=yourdomain,DC=com","YourAdminUser","YourAdminPassword");
            //    UserPrincipal up = new UserPrincipal(pricipalContext);
            //    up.SamAccountName = "yxl13153";
            //    up.DisplayName = "Test User";
            //    up.EmailAddress = "test@ineos.com";
            //    up.GivenName = "dump";
            //    up.Name = "Test User";
            //    up.Description = "User Created for testing";
            //    up.Enabled = true;
            //    up.SetPassword("Ineos2023");
            //    up.Save();
            //    MessageBox.Show("User Created");
            //}
            //catch (Exception ex)
            //{

            //}

            //try
            //{
            //    DirectoryEntry directoryEntry = new DirectoryEntry("");
            //    directoryEntry.Path = "LDAP://OU=Client,DC=in1,DC=ad,DC=innovene,DC=com";
            //    directoryEntry.AuthenticationType = AuthenticationTypes.Secure;


            //    DirectoryEntry childEntry = directoryEntry.Children.Add("CN=TestUser", "user");
            //    childEntry.Properties["samAccountName"].Value = "test12345";
            //    childEntry.Properties["mail"].Value = "test12345@ineos.com";
            //    childEntry.CommitChanges();
            //    directoryEntry.CommitChanges();
            //    childEntry.Invoke("SetPassword", new object[] { "Ineos2023" });
            //    childEntry.CommitChanges();
            //    MessageBox.Show("User Created");
            //}
            //catch (Exception ex)
            //{

            //}
        }
    }
}