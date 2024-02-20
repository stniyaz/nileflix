// add or remove favorite movie
var btns = document.querySelectorAll(".favorite-movie-btn");

btns.forEach(btn => btn.addEventListener("click", function (e) {
    e.preventDefault();
    var url = btn.getAttribute("href");

    fetch(url)
}))

// fav

const icons = document.querySelectorAll('.fav-icon');
function toggleIconClass(clickedIcon) {
    if (clickedIcon.classList.contains('fa-solid')) {
        clickedIcon.classList.remove('fa-solid');
        clickedIcon.classList.add('fa-regular');
    } else {
        clickedIcon.classList.remove('fa-regular');
        clickedIcon.classList.add('fa-solid');
    }
}

icons.forEach(icon => {
    icon.addEventListener('click', function () {
        $.ajax({
            type: "GET",
            url: "/home/isauthenticated",
            success: function (response) {
                if (response.loggedIn === true) {
                    toggleIconClass(icon);

                } else {
                    window.location.href = "account/signin";
                }
            },
            error: function (xhr, status, error) {
                console.error("Something went wrong:", error);
            }
        });
    });
});

// change email

document.getElementById("changeMail").addEventListener("click", function (e) {
    e.preventDefault();
    // Form verilerini al
    var formData = new FormData(document.getElementById("myForm"));

    // Ajax isteği yap
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "/Home/Detail", true); // Controller ve action belirtin
    xhr.onload = function () {
        if (xhr.status == 200) {
            // Status code 200 ise, işlem başarılı demektir
            Swal.fire({
                position: "top-end",
                icon: "success",
                title: "Changes saved. We sent an activation message to email address.",
                background: '#151f30',
                showConfirmButton: false,
                timer: 1500
            });
        } else if (xhr.status == 204) {
            Swal.fire({
                position: "top-end",
                icon: "success",
                title: "Changes saved.",
                background: '#151f30',
                showConfirmButton: false,
                timer: 2500
            });

            
        }
    };
    xhr.send(formData);
});

