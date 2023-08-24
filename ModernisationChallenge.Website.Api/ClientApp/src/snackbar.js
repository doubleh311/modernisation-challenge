export default function snackbar(className, message) {
    function snackbar_click() {
        var self = this;

        self.style.opacity = "0";

        setTimeout(function () {
            self.removeEventListener("click", snackbar_click);

            window.__snackbar_container.removeChild(self);

            if (window.__snackbar_container.childNodes.length == 0) {
                document.body.removeChild(window.__snackbar_container);

                delete window.__snackbar_container;
            }
        }, 150);
    }

    if (window.__snackbar_container == undefined) {
        function snackbar_container_mouseout() {
            window.__snackbar_container.__timeouts = [];

            for (let div of window.__snackbar_container.childNodes) {
                window.__snackbar_container.__timeouts.push(setTimeout(snackbar_click.bind(div), 5000));
            }
        }

        function snackbar_container_mouseover() {
            for (let timeout of window.__snackbar_container.__timeouts) {
                clearTimeout(timeout);
            }

            window.__snackbar_container.__timeouts = [];
        }

        window.__snackbar_container = document.createElement("div");

        window.__snackbar_container.__timeouts = [];

        window.__snackbar_container.className = "snackbar_container";

        document.body.appendChild(window.__snackbar_container);

        window.__snackbar_container.addEventListener("mouseout", snackbar_container_mouseout);
        window.__snackbar_container.addEventListener("mouseover", snackbar_container_mouseover);
    }

    var div = document.createElement("div");

    div.classList.add("snackbar");
    div.classList.add(className);
    div.innerText = message;

    div.addEventListener("click", snackbar_click);

    while (window.__snackbar_container.childNodes.length > 4) {
        window.__snackbar_container.removeChild(window.__snackbar_container.firstChild);
    }

    if (window.__snackbar_container.childNodes.length == 0) {
        window.__snackbar_container.appendChild(div);
    }
    else {
        window.__snackbar_container.insertBefore(div, window.__snackbar_container.childNodes[0]);
    }

    setTimeout(function () {
        div.style.opacity = "1";
    }, 1);

    window.__snackbar_container.__timeouts.push(setTimeout(snackbar_click.bind(div), 5000));
}