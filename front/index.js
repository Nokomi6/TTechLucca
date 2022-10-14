const forms = document.querySelectorAll('form');
const chats = document.querySelectorAll('.chat');


forms.forEach(form => {
    form.addEventListener('submit', (evt) => {
        evt.preventDefault();
        const input = form.querySelector('input');
        if (input.value !== '') {
            [].forEach.call(chats, function (chat, index) {
                const newP = document.createElement('p');
                if (index === 0) {
                    newP.className = input.id === "textA" ? "pRight" : "pLeft"
                    const whoSay = input.id === "textA" ? "Me: " : "B: "
                    newP.innerHTML = whoSay + input.value;
                } else {
                    newP.className = input.id === "textB" ? "pRight" : "pLeft"
                    const whoSay = input.id === "textB" ? "Me: " : "A: "
                    newP.innerHTML = whoSay + input.value;
                }
                chat.appendChild(newP);
                chat.scrollTo(0, chat.scrollHeight);
            });

        }
        input.value = '';
    })
});

