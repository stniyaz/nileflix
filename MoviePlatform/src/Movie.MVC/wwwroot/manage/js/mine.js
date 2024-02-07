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

