import { getMessaging} from 'https://www.gstatic.com/firebasejs/10.4.0/firebase-messaging-sw.js';
import { initializeApp } from 'https://www.gstatic.com/firebasejs/10.4.0/firebase-app.js';

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
const app = initializeApp(firebaseConfig);

const messaging = getMessaging(app);
import { getMessaging , onMessage} from 'https://www.gstatic.com/firebasejs/10.4.0/firebase-messaging.js';
const messaging = getMessaging();
onMessage(messaging, (payload) => {
    console.log('Message received. ', payload);
});