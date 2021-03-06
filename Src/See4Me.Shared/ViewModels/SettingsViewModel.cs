﻿using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using See4Me.Common;
using See4Me.Localization.Resources;
using See4Me.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using See4Me.Extensions;
using System.IO;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using See4Me.Engine;

namespace See4Me.ViewModels
{
    public partial class SettingsViewModel : ViewModelBase
    {
        private readonly ILauncherService launcherService;
        private readonly CognitiveClient cognitiveClient;

        public AutoRelayCommand RegisterCommand { get; set; }

        public AutoRelayCommand GotoAboutCommand { get; set; }

        public AutoRelayCommand GotoPrivacyPolicyCommand { get; set; }

        private string visionSubscriptionKey;
        public string VisionSubscriptionKey
        {
            get { return visionSubscriptionKey; }
            set { this.Set(ref visionSubscriptionKey, value); }
        }

        private string emotionSubscriptionKey;
        public string EmotionSubscriptionKey
        {
            get { return emotionSubscriptionKey; }
            set { this.Set(ref emotionSubscriptionKey, value); }
        }

        private string faceSubscriptionKey;
        public string FaceSubscriptionKey
        {
            get { return faceSubscriptionKey; }
            set { this.Set(ref faceSubscriptionKey, value); }
        }

        private string translatorSubscriptionKey;
        public string TranslatorSubscriptionKey
        {
            get { return translatorSubscriptionKey; }
            set { this.Set(ref translatorSubscriptionKey, value); }
        }

        private bool isTextToSpeechEnabled;
        public bool IsTextToSpeechEnabled
        {
            get { return isTextToSpeechEnabled; }
            set { this.Set(ref isTextToSpeechEnabled, value); }
        }

        private bool showDescriptionConfidence;
        public bool ShowDescriptionConfidence
        {
            get { return showDescriptionConfidence; }
            set { this.Set(ref showDescriptionConfidence, value); }
        }

        private bool showOriginalDescriptionOnTranslation;
        public bool ShowOriginalDescriptionOnTranslation
        {
            get { return showOriginalDescriptionOnTranslation; }
            set { this.Set(ref showOriginalDescriptionOnTranslation, value); }
        }

        private bool showDescriptionOnFaceIdentification;
        public bool ShowDescriptionOnFaceIdentification
        {
            get { return showDescriptionOnFaceIdentification; }
            set { this.Set(ref showDescriptionOnFaceIdentification, value); }
        }

        public SettingsViewModel(CognitiveClient cognitiveClient, ILauncherService launcherService)
        {
            this.cognitiveClient = cognitiveClient;
            this.launcherService = launcherService;

            this.CreateCommands();
        }

        public void Initialize()
        {
            VisionSubscriptionKey = ServiceKeys.VisionSubscriptionKey;
            EmotionSubscriptionKey = ServiceKeys.EmotionSubscriptionKey;
            FaceSubscriptionKey = ServiceKeys.FaceSubscriptionKey;
            TranslatorSubscriptionKey = ServiceKeys.TranslatorSubscriptionKey;

            IsTextToSpeechEnabled = Settings.IsTextToSpeechEnabled;
            ShowDescriptionConfidence = Settings.ShowDescriptionConfidence;
            ShowOriginalDescriptionOnTranslation = Settings.ShowOriginalDescriptionOnTranslation;
            showDescriptionOnFaceIdentification = Settings.ShowDescriptionOnFaceIdentification;
        }

        private void CreateCommands()
        {
            RegisterCommand = new AutoRelayCommand(() => launcherService.LaunchUriAsync(Constants.HowToRegisterUrl));
            GotoAboutCommand = new AutoRelayCommand(() => AppNavigationService.NavigateTo(Pages.AboutPage.ToString()));
            GotoPrivacyPolicyCommand = new AutoRelayCommand(() => AppNavigationService.NavigateTo(Pages.PrivacyPolicyPage.ToString()));
        }

        public void Save()
        {
            ServiceKeys.VisionSubscriptionKey = visionSubscriptionKey;
            ServiceKeys.EmotionSubscriptionKey = emotionSubscriptionKey;
            ServiceKeys.FaceSubscriptionKey = faceSubscriptionKey;
            ServiceKeys.TranslatorSubscriptionKey = translatorSubscriptionKey;

            Settings.IsTextToSpeechEnabled = isTextToSpeechEnabled;
            Settings.ShowDescriptionConfidence = showDescriptionConfidence;
            Settings.ShowOriginalDescriptionOnTranslation = showOriginalDescriptionOnTranslation;
            Settings.ShowDescriptionOnFaceIdentification = showDescriptionOnFaceIdentification;

            var cognitiveSettings = cognitiveClient.Settings;
            cognitiveSettings.EmotionSubscriptionKey = emotionSubscriptionKey;
            cognitiveSettings.VisionSubscriptionKey = visionSubscriptionKey;
            cognitiveSettings.FaceSubscriptionKey = faceSubscriptionKey;
            cognitiveSettings.TranslatorSubscriptionKey = translatorSubscriptionKey;
        }
    }
}
