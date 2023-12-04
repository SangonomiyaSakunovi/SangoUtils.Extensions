namespace SangoUtils_Server
{
    public class SecurityCheckService : BaseService<SecurityCheckService>
    {
        private SecurityCheckServiceConfig _securityCheckServiceConfig = null;

        private string _limitTimestampKey = "key1";
        private string _lastRunTimestampKey = "key2";

        private float _currentTickTime = 1;
        private float _maxTickTime = 60;

        private bool _isApplicationRunValid = false;

        public void OnInit(SecurityCheckServiceConfig config)
        {
            base.Init();
            if (config != null)
            {
                _securityCheckServiceConfig = config;
            }
        }

        protected override void OnUpdate()
        {
            base.Update();
            if (_isApplicationRunValid)
            {
                if (_currentTickTime > 0)
                {
                    //_currentTickTime -= Time.deltaTime;
                }
                else
                {
                    _currentTickTime = _maxTickTime;
                    TickUpdateRunTime();
                }
            }
        }

        public void UpdateRegistInfo(string registLimitTimestampNew, string signData)
        {
            if (string.IsNullOrEmpty(registLimitTimestampNew) || string.IsNullOrEmpty(signData))
            {
                _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateError_NullInfo);
                return;
            }
            if (!long.TryParse(registLimitTimestampNew, out long result))
            {
                _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateError_SyntexError);
                return;
            }
            switch (_securityCheckServiceConfig.registMixSignDataProtocol)
            {
                case RegistMixSignDataProtocol.SIGN:
                    SecurityCheckMapSango.CheckProtocl_SIGNDATA(registLimitTimestampNew, signData, _securityCheckServiceConfig, WriteRegistInfo);
                    break;
            }
        }

        public void UpdateRegistInfo(string mixSignData)
        {
            if (string.IsNullOrEmpty(mixSignData))
            {
                _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateError_NullInfo);
                return;
            }
            switch (_securityCheckServiceConfig.registMixSignDataProtocol)
            {
                case RegistMixSignDataProtocol.A_B_C_SIGN:
                    SecurityCheckMapSango.CheckProtocol_A_B_C_SIGN(mixSignData, _securityCheckServiceConfig, WriteRegistInfo);
                    break;
            }
        }

        private void WriteRegistInfo(string registLimitTimestampNew)
        {
            long nowTimestamp = TimeUtils.GetUnixDateTimeSeconds(DateTime.Now);
            if (Convert.ToInt64(registLimitTimestampNew) > nowTimestamp)
            {
                string registLimitTimestampDataNew = TimeCryptoUtils.EncryptTimestamp(registLimitTimestampNew);
                string registLastRunTimestampDataNew = TimeCryptoUtils.EncryptTimestamp(nowTimestamp);
                bool res1 = PersistDataService.Instance.AddPersistData(_limitTimestampKey, registLimitTimestampDataNew);
                bool res2 = PersistDataService.Instance.AddPersistData(_lastRunTimestampKey, registLastRunTimestampDataNew);
                if (res1 && res2)
                {
                    _isApplicationRunValid = true;
                    _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateOK_Success);
                }
                else
                {
                    _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateFailed_WriteInfoError);
                }
            }
            else
            {
                Console.WriteLine("RegistFaild, the NewRegistLimitTimestamp should newer than NowTimestamp.");
                _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateFailed_OutData);
            }
        }

        public void CheckRegistValidation()
        {
            long nowTimestamp = TimeUtils.GetUnixDateTimeSeconds(DateTime.Now);
            long defaultRegistLimitTimestamp = TimeUtils.GetUnixDateTimeSeconds(_securityCheckServiceConfig.defaultRegistLimitDateTime);

            string registLimitTimestampData = PersistDataService.Instance.GetPersistData(_limitTimestampKey);
            string registLastRunTimestampData = PersistDataService.Instance.GetPersistData(_lastRunTimestampKey);
            Console.WriteLine("Now is time to Find the RegistInfo, please wait....................................");
            Console.WriteLine("The RegistLimitTimestampInfo Found: [ " + registLimitTimestampData + " ]");
            Console.WriteLine("The LastRunTimestampInfo Found: [ " + registLastRunTimestampData + " ]");

            if (string.IsNullOrEmpty(registLimitTimestampData) || string.IsNullOrEmpty(registLastRunTimestampData))
            {
                bool res = false;
                registLimitTimestampData = TimeCryptoUtils.EncryptTimestamp(defaultRegistLimitTimestamp);
                registLastRunTimestampData = TimeCryptoUtils.EncryptTimestamp(nowTimestamp);
                Console.WriteLine("That`s the First Time open this software, we give the default registLimitTimestamp is: [ " + defaultRegistLimitTimestamp + " ]");
                bool res1 = PersistDataService.Instance.AddPersistData(_limitTimestampKey, registLimitTimestampData);
                bool res2 = PersistDataService.Instance.AddPersistData(_lastRunTimestampKey, registLastRunTimestampData);
                if (res1 && res2)
                {
                    res = true;
                    _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.CheckOK_FirstRun);
                }
                else
                {
                    _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateFailed_WriteInfoError);
                }
                Console.WriteLine("Is first regist OK? [ " + res + " ]");
            }
            else
            {
                long registLimitTimestamp = Convert.ToInt64(TimeCryptoUtils.DecryptTimestamp(registLimitTimestampData));
                long registLastRunTimestamp = Convert.ToInt64(TimeCryptoUtils.DecryptTimestamp(registLastRunTimestampData));
                Console.WriteLine("We DeCrypt the RegistInfo, please wait....................................");
                Console.WriteLine("The RegistLimitTimestamp is: [ " + registLimitTimestamp + " ]");
                Console.WriteLine("The LastRunTimestamp is: [ " + registLastRunTimestamp + " ]");
                Console.WriteLine("The NowTimestamp is: [ " + nowTimestamp + " ]");
                if (nowTimestamp < registLastRunTimestamp)
                {
                    Console.WriteLine("Error: SystemTime has in Changed");
                    _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.CheckError_SystemTimeChanged);
                }
                else
                {
                    if (nowTimestamp < registLimitTimestamp)
                    {
                        _isApplicationRunValid = true;
                        _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.CheckOK_Valid);
                    }
                    else
                    {
                        _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.CheckFailed_OutData);
                    }
                }
            }
        }

        public void GetNewRegistInfo(string rawData)
        {
            string signData = "";
            switch (_securityCheckServiceConfig.signMethodCode)
            {
                case SignMethodCode.Md5:
                    signData = Md5SignatureUtils.GenerateMd5SignData(rawData, _securityCheckServiceConfig.secretTimestamp, _securityCheckServiceConfig.apiKey, _securityCheckServiceConfig.apiSecret, 0);
                    break;
            }
            Console.WriteLine("Generate New SignRegistInfo, please wait....................................");
            Console.WriteLine("====================SignRawData====================");
            Console.WriteLine(rawData);
            Console.WriteLine("==================================================");
            Console.WriteLine("SignMethod: [ " + _securityCheckServiceConfig.signMethodCode + " ]");
            Console.WriteLine("====================SignedData====================");
            Console.WriteLine(signData);
            Console.WriteLine("==================================================");
        }

        private void TickUpdateRunTime()
        {
            long nowTimestamp = TimeUtils.GetUnixDateTimeSeconds(DateTime.Now);
            string registLastRunTimestampData = PersistDataService.Instance.GetPersistData(_lastRunTimestampKey);
            if (string.IsNullOrEmpty(registLastRunTimestampData))
            {
                return;
            }
            long lastRunTimetamp = Convert.ToInt64(TimeCryptoUtils.DecryptTimestamp(registLastRunTimestampData));
            if (lastRunTimetamp > nowTimestamp)
            {
                return;
            }
            else
            {
                string registLastRunTimestampDataNew = TimeCryptoUtils.EncryptTimestamp(nowTimestamp);
                PersistDataService.Instance.AddPersistData(_lastRunTimestampKey, registLastRunTimestampDataNew);
            }
        }
    }

    public enum SignMethodCode
    {
        Md5
    }

    public enum RegistInfoCode
    {
        Timestamp
    }

    public enum RegistInfoCheckResult
    {
        CheckOK_Valid,
        CheckOK_FirstRun,
        CheckFailed_OutData,
        CheckError_SystemTimeChanged,
        UpdateOK_Success,
        UpdateFailed_OutData,
        UpdateFailed_SignError,
        UpdateFailed_WriteInfoError,
        UpdateError_NullInfo,
        UpdateError_SyntexError,
        UpdateError_LenghthError
    }

    public class SecurityCheckServiceConfig
    {
        public string apiKey;
        public string apiSecret;
        public string secretTimestamp;
        public DateTime defaultRegistLimitDateTime;
        public SignMethodCode signMethodCode;
        public RegistInfoCode registInfoCode;
        public int checkLength;
        public RegistMixSignDataProtocol registMixSignDataProtocol;
        public Action<RegistInfoCheckResult> resultActionCallBack;
    }
}
