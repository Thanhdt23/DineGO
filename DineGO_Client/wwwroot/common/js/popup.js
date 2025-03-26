//Popup_Form - Thang - Start
document.addEventListener("DOMContentLoaded", function () {
    let form = document.getElementById("dynamicForm");

    document.querySelectorAll(".open-dynamic-modal").forEach(button => {
        button.addEventListener("click", function () {
            let title = this.getAttribute("data-title");
            let fields = this.getAttribute("data-fields").split(",");
            let action = this.getAttribute("data-action");
            let controller = this.getAttribute("data-controller");
            let method = this.getAttribute("data-method").toUpperCase(); // Lấy method GET/POST

            // Lưu thông tin vào form để xử lý submit
            form.setAttribute("data-action", action);
            form.setAttribute("data-controller", controller);
            form.setAttribute("data-method", method);

            document.getElementById("dynamicModalLabel").textContent = title;
            let formFieldsContainer = document.getElementById("formFields");
            formFieldsContainer.innerHTML = "";

            fields.forEach(field => {
                let label = document.createElement("label");
                label.textContent = field.charAt(0).toUpperCase() + field.slice(1) + ":";
                label.classList.add("form-label");

                let input = document.createElement("input");
                input.type = field === "password" ? "password" : "text";
                input.classList.add("form-control");
                input.name = field;
                input.id = field;

                let div = document.createElement("div");
                div.classList.add("mb-3");
                div.appendChild(label);
                div.appendChild(input);

                formFieldsContainer.appendChild(div);
            });

            let modal = new bootstrap.Modal(document.getElementById("dynamicModal"));
            modal.show();
        });
    });

    // Xử lý sự kiện submit
    form.addEventListener("submit", function (event) {
        event.preventDefault();

        let formData = new FormData(this);
        let action = this.getAttribute("data-action");
        let controller = this.getAttribute("data-controller");
        let method = this.getAttribute("data-method");

        let url = `/${controller}/${action}`;

        if (method === "GET") {
            // Chuyển trang với query string
            let params = new URLSearchParams([...formData]).toString();
            window.location.href = `${url}?${params}`;
        } else {
            // Mặc định là POST
            fetch(url, {
                method: "POST",
                body: new URLSearchParams([...formData])
            })
                .then(response => {
                    if (response.redirected) {
                        window.location.href = response.url; // Chuyển trang nếu có redirect
                    } else {
                        return response.text();
                    }
                })
                .then(data => console.log("POST Response:", data))
                .catch(error => console.error("Lỗi:", error));
        }
    });
});
//Popup_Form - Thang - End