// genre

var btns = document.querySelectorAll(".delete-btn");

btns.forEach(btn => btn.addEventListener("click", function (e) {
    e.preventDefault();
    let url = btn.getAttribute("href");

    Swal.fire({
        title: "Are you sure?",
        text: "This genre and all movies in this genre will be removed. This action cannot be undone.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#014709",
        cancelButtonColor: "#8b0000",
        confirmButtonText: "Yes, delete it!",
        background: '#151f30',

    }).then((result) => {
        if (result.isConfirmed) {
            fetch(url)
                .then(response => {
                    if (response.status == 200)
                        window.location.reload(true);
                })
        }
    });

}))

// movie

var movieBtns = document.querySelectorAll(".delete-btn-movie");

movieBtns.forEach(btn => btn.addEventListener("click", function (e) {
    e.preventDefault();
    let url = btn.getAttribute("href");

    Swal.fire({
        title: "Are you sure?",
        text: "The movie will be deleted. This action cannot be undone.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#014709",
        cancelButtonColor: "#8b0000",
        confirmButtonText: "Yes, delete it!",
        background: '#151f30',

    }).then((result) => {
        if (result.isConfirmed) {
            fetch(url)
                .then(response => {
                    if (response.status == 200)
                        window.location.reload(true);
                })
        }
    });

}))

// user

var userBtns = document.querySelectorAll(".delete-btn-user");

userBtns.forEach(userbtn => userbtn.addEventListener("click", function (e) {
    e.preventDefault();
    let url = userbtn.getAttribute("href");

    Swal.fire({
        title: "Are you sure?",
        text: "The movie will be deleted. This action cannot be undone.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#014709",
        cancelButtonColor: "#8b0000",
        confirmButtonText: "Yes, delete it!",
        background: '#151f30',

    }).then((result) => {
        if (result.isConfirmed) {
            fetch(url)
                .then(response => {
                    if (response.status == 200)
                        window.location.reload(true);
                })
        }
    });

}))

// comment
var commentBtns = document.querySelectorAll(".delete-cmnt");

commentBtns.forEach(cmnt => cmnt.addEventListener("click", function (e) {
    e.preventDefault();
    let url = cmnt.getAttribute("href");

    Swal.fire({
        title: "Are you sure?",
        text: "The movie will be deleted. This action cannot be undone.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#014709",
        cancelButtonColor: "#8b0000",
        confirmButtonText: "Yes, delete it!",
        background: '#151f30',

    }).then((result) => {
        if (result.isConfirmed) {
            fetch(url)
                .then(response => {
                    if (response.status == 200)
                        window.location.reload(true);
                })
        }
    });

}))

// live delete
var liveBtns = document.querySelectorAll(".delete-live-btn");

liveBtns.forEach(lbtn => lbtn.addEventListener("click", function (e) {
    e.preventDefault();
    let url = lbtn.getAttribute("href");

    Swal.fire({
        title: "Are you sure?",
        text: "The live will be deleted. This action cannot be undone.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#014709",
        cancelButtonColor: "#8b0000",
        confirmButtonText: "Yes, delete it!",
        background: '#151f30',

    }).then((result) => {
        if (result.isConfirmed) {
            fetch(url)
                .then(response => {
                    if (response.status == 200)
                        window.location.reload(true);
                })
        }
    });

}))