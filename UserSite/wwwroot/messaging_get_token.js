import { initializeApp } from './js/firebase-app.js';
import { getMessaging , getToken} from './js/firebase-messaging.js';

// TODO: Replace the following with your app's Firebase project configuration
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

const vapidKey = "BO5o60r4w63Q7wDRSNUKnV5QLoQeG6QdWtoJh1B_RMTkiMt5yhzxhbkaqod1vccIdMHIFII_0mIBdWeia6BNQDg";

console.log("have app");
const messaging = getMessaging(app);

console.log("have messaging", messaging);

function requestPermission() {
    console.log('Requesting permission...');
    Notification.requestPermission().then((permission) => {
        if (permission === 'granted') {
            console.log('Notification permission granted.');
        }
    })
}
requestPermission()
getToken(messaging, { vapidKey })
    .then((res) => {
        console.log("token", res);
        if (res !== null) {
            // Perform AJAX GET request
            $.ajax({
                url: '/AuthPage/CollectDeviceToken/',
                type: 'GET',
                data:{deviceToken:res},
                success: function(data) {
                    console.log(data);
                },
                error: function(error) {
                    console.error(error);
                }
            });
        }
    })
    .catch((err) => console.error(err));

