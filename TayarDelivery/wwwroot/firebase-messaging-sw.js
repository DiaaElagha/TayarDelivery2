importScripts('https://www.gstatic.com/firebasejs/8.1.2/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/8.1.2/firebase-messaging.js');
importScripts('https://www.gstatic.com/firebasejs/8.1.2/firebase-analytics.js');

var firebaseConfig = {
    apiKey: "AIzaSyCUvQx4-sChtVfKE9Ne4z2f6wL0V0tyuoM",
    authDomain: "tayar-338fd.firebaseapp.com",
    projectId: "tayar-338fd",
    storageBucket: "tayar-338fd.appspot.com",
    messagingSenderId: "349804934208",
    appId: "1:349804934208:web:4ff6c941cc9cad9fb11ba5",
    measurementId: "G-EPRHDGNHB3"
};

firebase.initializeApp(firebaseConfig);
firebase.analytics();

const messaging = firebase.messaging();

messaging.onBackgroundMessage(function (payload) {
    console.log('[firebase-messaging-sw.js] Received background message ', payload);
    // Customize notification here
    const notificationTitle = 'Background Message Title';
    const notificationOptions = {
        body: 'Background Message body.',
        icon: '/firebase-logo.png'
    };

    self.registration.showNotification(notificationTitle,
        notificationOptions);
});
