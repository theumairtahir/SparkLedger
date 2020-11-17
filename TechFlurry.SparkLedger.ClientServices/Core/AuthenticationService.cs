using System;
using System.Linq;
using TechFlurry.SparkLedger.ApplicationDomain.Elements;
using TechFlurry.SparkLedger.ApplicationDomain.EventArgs;
using TechFlurry.SparkLedger.BusinessDomain.ViewModels;
using TechFlurry.SparkLedger.ClientServices.Abstractions;
using TechFlurry.SparkLedger.Shared.Common;
using TechFlurry.SparkLedger.Shared.Extentions;
using TechFlurry.SparkLedger.Shared.Helpers;
using TG.Blazor.IndexedDB;

namespace TechFlurry.SparkLedger.ClientServices.Core
{
    public interface IAuthenticationService : IValueUpdator
    {
        bool IsAuthenticated { get; }
        string UserId { get; }
        int VerificationStep { get; }
        string Phone { get; set; }
        string UserFirstName { get; set; }
        string UserSecondName { get; set; }
        string UserImage { get; }
        string Token { get; }

        event EventHandler<AuthenticationEventArgs> OnAuthenticated;
        event EventHandler<AuthenticationEventArgs> OnLogout;
        event EventHandler<ApplicationEventArgs> OnTokenGenerated;

        void AuthenticateUser(string token);
        void CheckAuthentication();
        void InitializeAuthentication(string token);
        void Logout();
        void NextAuthenticationStep();
        void SubmitUser();
    }

    internal class AuthenticationService : IAuthenticationService
    {
        private bool isAuthenticated;
        private readonly IndexedDBManager _dbManager;

        public AuthenticationService(IndexedDBManager dbManager)
        {
            isAuthenticated = false;
            VerificationStep = 1;
            _dbManager = dbManager;
        }
        public string UserId { get; private set; }
        public string Phone { get; set; }
        public string UserFirstName { get; set; }
        public string UserImage { get; private set; }
        public string UserSecondName { get; set; }
        public int VerificationStep { get; private set; }
        public string Token { get; private set; }
        public bool IsAuthenticated
        {
            get
            {
                return isAuthenticated;
            }
            private set
            {
                isAuthenticated = value;
                OnValueUpdate.Invoke(this, new OnUpdateEventArgs
                {
                    CallerType = GetType(),
                    CallingMethod = nameof(IsAuthenticated),
                    CallingObject = this
                });
                //if (value)
                //{
                //    try
                //    {
                //        OnAuthenticated.Invoke(this, new AuthenticationEventArgs
                //        {
                //            CallerType = GetType(),
                //            CallingMethod = "IsAuthenticated",
                //            CallingObject = this
                //        });
                //    }
                //    catch (Exception ex)
                //    {

                //        throw;
                //    }
                //}
                //else
                //{
                //    try
                //    {
                //        OnLogout.Invoke(this, new AuthenticationEventArgs
                //        {
                //            CallingObject = this,
                //            CallingMethod = "IsAuthenticated",
                //            CallerType = GetType()

                //        });
                //    }
                //    catch (Exception ex)
                //    {

                //        throw;
                //    }
                //}
            }
        }

        public event EventHandler<OnUpdateEventArgs> OnValueUpdate;
        public event EventHandler<ApplicationEventArgs> OnTokenGenerated;
        public event EventHandler<AuthenticationEventArgs> OnAuthenticated;
        public event EventHandler<AuthenticationEventArgs> OnLogout;
        public void AuthenticateUser(string userId)
        {
            UserId = userId;
            //find uId in db, if found then return the user and jump to verification step 4 and generate token else take the new user Info
            VerificationStep++;
            OnValueUpdate.Invoke(this, new OnUpdateEventArgs
            {
                CallerType = GetType(),
                CallingMethod = nameof(InitializeAuthentication),
                CallingObject = this
            });
            //GenerateToken();
        }
        public void GenerateToken()
        {
            var userInfo = new UserInfoModel
            {
                FullName = new BusinessDomain.ValueObjects.Name
                {
                    FirstName = UserFirstName,
                    LastName = UserSecondName
                },
                PhoneNumber = Phone,
                PicPath = UserImage,
                UserId = UserId
            };
            var json = userInfo.ToJson();
            Token = Cryptography.Encrypt(json);
            Functions.RunOnThread(SaveTokenToLocalDb);
        }
        private async void SaveTokenToLocalDb()
        {
            var newRecord = new StoreRecord<Token>
            {
                Storename = "Tokens",
                Data = new Token
                {
                    Id = 1,
                    Value = Token
                }
            };
            await _dbManager.ClearStore("Tokens");
            await _dbManager.AddRecord(newRecord);
        }
        public void NextAuthenticationStep()
        {
            VerificationStep++;
            OnValueUpdate.Invoke(this, new OnUpdateEventArgs
            {
                CallerType = GetType(),
                CallingMethod = nameof(NextAuthenticationStep),
                CallingObject = this
            });
        }
        public void InitializeAuthentication(string token)
        {
            try
            {
                var json = Cryptography.Decrypt(token);
                var userInfo = json.ToObject<UserInfoModel>();
                UserId = userInfo.UserId;
                UserImage = userInfo.PicPath;
                Phone = userInfo.PhoneNumber;
                UserFirstName = userInfo.FullName.FirstName;
                UserSecondName = userInfo.FullName.LastName;
                IsAuthenticated = true;
            }
            catch (Exception)
            {
                IsAuthenticated = false;
            }
        }
        public async void CheckAuthentication()
        {
            var token = (await _dbManager.GetRecords<Token>("Tokens")).FirstOrDefault();
            if (token != null)
            {
                InitializeAuthentication(token.Value);
            }
        }
        public void SubmitUser()
        {
            IsAuthenticated = true;
            UserImage = "assets/media/users/300_21.jpg";
            OnValueUpdate.Invoke(this, new OnUpdateEventArgs
            {
                CallerType = GetType(),
                CallingMethod = nameof(SubmitUser),
                CallingObject = this
            });
            GenerateToken();
        }
        public void Logout()
        {
            IsAuthenticated = false;
        }
    }
}
