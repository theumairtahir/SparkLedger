using System;
using System.Collections.Generic;
using System.Text;
using TechFlurry.SparkLedger.ApplicationDomain.EventArgs;
using TechFlurry.SparkLedger.ClientServices.Abstractions;

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

        event EventHandler<AuthenticationEventArgs> OnAuthenticated;
        event EventHandler<AuthenticationEventArgs> OnLogout;

        void AuthenticateUser(string userId);
        void Logout();
        void NextAuthenticationStep();
        void SubmitUser();
    }

    internal class AuthenticationService : IAuthenticationService
    {
        private bool isAuthenticated;
        public AuthenticationService()
        {
            isAuthenticated = false;
            VerificationStep = 1;
        }
        public string UserId { get; private set; }
        public string Phone { get; set; }
        public string UserFirstName { get; set; }
        public string UserImage { get; private set; }
        public string UserSecondName { get; set; }
        public int VerificationStep { get; private set; }
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
        public event EventHandler<AuthenticationEventArgs> OnAuthenticated;
        public event EventHandler<AuthenticationEventArgs> OnLogout;
        public void AuthenticateUser(string userId)
        {
            UserId = userId;
            //find uId in db, if found then return the user and jump to verification step 4 else take the new user Info
            VerificationStep++;
            OnValueUpdate.Invoke(this, new OnUpdateEventArgs
            {
                CallerType = GetType(),
                CallingMethod = nameof(InitializeAuthentication),
                CallingObject = this
            });
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
        public void InitializeAuthentication(string uId)
        {
            UserId = uId;
            UserImage = "assets/media/users/300_21.jpg";
            IsAuthenticated = true;
            OnValueUpdate.Invoke(this, new OnUpdateEventArgs
            {
                CallerType = GetType(),
                CallingMethod = nameof(AuthenticateUser),
                CallingObject = this
            });
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
        }
        public void Logout()
        {
            IsAuthenticated = false;
        }
    }
}
