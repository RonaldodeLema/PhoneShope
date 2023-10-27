importScripts('https://www.gstatic.com/firebasejs/8.10.1/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/8.10.1/firebase-messaging.js');
const firebaseConfig = {
    apiKey: "AIzaSyAOdCMN15VryQG3hkPTnNtSNsmn_tk3mF8",
    authDomain: "push-notify-87a05.firebaseapp.com",
    projectId: "push-notify-87a05",
    storageBucket: "push-notify-87a05.appspot.com",
    messagingSenderId: "780951736071",
    appId: "1:780951736071:web:24e05166eed9818badbfa2",
    measurementId: "G-9LF3KB9FNB"
};

// Initialize Firebase
const app = firebase.initializeApp(firebaseConfig);
const messaging = firebase.messaging();
messaging.onBackgroundMessage((payload) => {
    console.log(
        '[firebase-messaging-sw.js] Received background message ',
        payload
    );
    // Customize notification here
    const notificationTitle = 'You have a new message from '+payload.from;
    const notificationOptions = {
        body: payload.data.FirstName+" "+payload.data.LastName +", Hello my bro",
        icon: 'img/logo.png'
    };
    self.registration.showNotification(notificationTitle, notificationOptions);
});