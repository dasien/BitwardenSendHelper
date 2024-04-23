using System.Diagnostics;
using BitwardenSendHelper.Models;
using Newtonsoft.Json;

namespace BitwardenSendHelper.Utils
{
    public class CLIProxy
    {
        private readonly Process _bwcli;
        private string _response;
        private string _error;
        private string _sessionKey;
        private string _cmdExitCode;
        private bool _isLocked;
        private bool _isLoggedIn;

        public CLIProxy(string bwExeLocation)
        {
            // Set defaults.
            _response = "";
            _cmdExitCode = "";
            _sessionKey = "";
            _error = "";
            _isLocked = true;
            _isLoggedIn = false;
            
            // Create exe caller.
            _bwcli = new Process();
            _bwcli.StartInfo.FileName = bwExeLocation;

            // Set call options
            _bwcli.EnableRaisingEvents = true;
            _bwcli.StartInfo.UseShellExecute = false;
            _bwcli.StartInfo.RedirectStandardError = true;
            _bwcli.StartInfo.RedirectStandardOutput = true;

            // Attach events.
            _bwcli.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(proxyCmd_DidReceiveData);
            _bwcli.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(proxyCmd_DidReceiveError);
            _bwcli.Exited += new System.EventHandler(proxyCmd_DidExit);

        }
        
         #region Auth Methods
        public void Login(string email, string password)
        {
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("login");
            _bwcli.StartInfo.ArgumentList.Add(email);
            _bwcli.StartInfo.ArgumentList.Add(password);

            // Execute.
            ExecuteCommand();

            // Check to see if the command worked.
            if (_cmdExitCode == "0")
            {
                // Get session key from response.
                string marker = "--session ";

                // Make sure the marker is in the response.
                if (_response.Contains(marker))
                {
                    // Find the start of the key
                    int pos = _response.IndexOf(marker, 0) + marker.Length;

                    // Store the session key for future calls.
                    _sessionKey = _response.Substring(pos);

                    // Set env variable.
                    _bwcli.StartInfo.Environment.Add("BW_SESSION", _sessionKey);

                    // At this point we are logged in.
                    _isLoggedIn = true;
                    _isLocked = false;
                }
            }
        }
        
        public void Logout()
        {
            // Check to make sure we are logged in.
            if (_isLoggedIn)
            {
                // Add parameters for call.
                _bwcli.StartInfo.ArgumentList.Add("logout");
                
                // Execute.
                ExecuteCommand();

                // Check to see if the command worked.
                if (_cmdExitCode == "0")
                {
                    // Flip login bit.
                    _isLoggedIn = false;

                    // Flip locked bit.
                    _isLocked = true;
                }
            }
        }

        public void Unlock(string password)
        {
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("unlock");
            _bwcli.StartInfo.ArgumentList.Add(password);

            // Execute.
            ExecuteCommand();

            // Check to see if the command worked.
            if (_cmdExitCode == "0")
            {
                // Get session key from response.
                string marker = "--session ";

                // Make sure the marker is in the response.
                if (_response.Contains(marker))
                {
                    // Find the start of the key
                    int pos = _response.IndexOf(marker, 0) + marker.Length;

                    // Store the session key for future calls.
                    _sessionKey = _response.Substring(pos);

                    // Set env variable.
                    _bwcli.StartInfo.Environment.Add("BW_SESSION", _sessionKey);

                    // At this point we are unlocked.
                    _isLocked = false;
                }
            }
        }

        public void Lock()
        {
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("lock");

            // Execute.
            ExecuteCommand();
            
            // Check to see if the command worked.
            if (_cmdExitCode == "0")
            {
                // Flip locked bit.
                _isLocked = true;
            }
        }
        #endregion
        
        #region Send Methods

        public SendResponse SendFile(string valueToSend, string? name, int? expiryDays, int? maxAccessCount, bool hidden, string? notes )
        {
            // The return value.
            SendResponse retVal = null;
            
            // Add parameters for call.
            _bwcli.StartInfo.ArgumentList.Add("send");
            _bwcli.StartInfo.ArgumentList.Add("--file");
            
            // Check to see if a name was added.
            if (name != null)
            {
                _bwcli.StartInfo.ArgumentList.Add("--name");
                _bwcli.StartInfo.ArgumentList.Add(name);
            }

            // Check to see if the user set a days to expire.
            if (expiryDays != null)
            {
                _bwcli.StartInfo.ArgumentList.Add("--deleteInDays");
                _bwcli.StartInfo.ArgumentList.Add(expiryDays.ToString());
            }

            // Check to see if there is a max number of accesses.
            if (maxAccessCount != null)
            {
                _bwcli.StartInfo.ArgumentList.Add("--maxAccessCount");
                _bwcli.StartInfo.ArgumentList.Add(maxAccessCount.ToString());
            }

            // Check to see if the text should be hidden.
            if (hidden)
            {
                _bwcli.StartInfo.ArgumentList.Add("--hidden");
            }

            // Check to see if notes were added.
            if (notes != null)
            {
                _bwcli.StartInfo.ArgumentList.Add("--notes");
                _bwcli.StartInfo.ArgumentList.Add(notes);
            }
            
            // Add the file name to command.
            _bwcli.StartInfo.ArgumentList.Add(valueToSend);
            
            // Execute.
            ExecuteCommand();
            
            // Check to make sure it didn't error out.
            if (_cmdExitCode == "0")
            {
                // Set serialization rules.
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;

                // Get item list.
                retVal = JsonConvert.DeserializeObject<SendResponse>(_response, settings);
            }

            // Return password.
            return retVal;
        }
        #endregion
        
        #region CLI Execute Wrapper
        private void ExecuteCommand()
        {
            // Reset command flags & response.
            _cmdExitCode = "";
            _response = "";
            _error = "";

            // Execute.
            _bwcli.Start();
            
            // Event starting.
            _bwcli.BeginErrorReadLine();
            _bwcli.BeginOutputReadLine();

            // Block until finished.
            _bwcli.WaitForExit();
            
            // Close streams.
            _bwcli.CancelErrorRead();
            _bwcli.CancelOutputRead();
            
            // Reset arguments.
            _bwcli.StartInfo.ArgumentList.Clear();
        }
        #endregion
        
        #region Event Handlers
        private void proxyCmd_DidExit(object sender, EventArgs e)
        {
            // Flag that data has all been received.
            _cmdExitCode = _bwcli.ExitCode.ToString();
        }

        private void proxyCmd_DidReceiveError(object sender, DataReceivedEventArgs e)
        {
            _error += e.Data;
        }

        private void proxyCmd_DidReceiveData(object sender, DataReceivedEventArgs e)
        {
            // Append to response.
            _response += e.Data;
        }
        #endregion
        
        #region Properties
        public string ExitCode
        {
            get
            {
                return _cmdExitCode;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _error;
            }
        }
        
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
        }

        public bool IsLocked
        {
            get { return _isLocked; }
        }
        #endregion
    }    
}

