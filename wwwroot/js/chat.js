class Message {
    constructor(username, message) {
        this.username = username;
        this.message = message;
        this.dateSent = dateSent
    }
}

const username = username;
const message = document.getElementById('message');
const chat = document.getElementById('chat');
const messageQueue = [];

function sendMessage() {
    let text = messageQueue.shift() || "";
    if (text.trim() === "") return;

    let messageBody = new Message(username, message)
    sendMessageToHub(messageBody);
}

function addMessageToChat(message) {
    let isCurrentUserMessage = message.username === username;
    let container = document.createElement('div');
    container.className = isCurrentUserMessage ? "container darker" : "container";

    let sender = document.createElement('p');
    sender.className = "sender";
    sender.innerHTML = message.username;

    let messageBody = document.createElement('p');
    messageBody.innerHTML = message.message;

    let dateSent = document.createElement('span');
    dateSent.className = isCurrentUserMessage ? "time-left" : "time-right";

    var currentDate = new Date();
    dateSent.innerHTML = currentDate;

    container.appendChild(sender);
    container.appendChild(messageBody);
    container.appendChild(dateSent);

    chat.appendChild(container);
}