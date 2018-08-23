$(function () {
	var $canvas = $("#canvas"),
		context = $canvas.get(0).getContext('2d');


	$("#saveServer").click(function () {
		
		var croppedImage = $canvas.cropper('getCroppedCanvas').toDataURL('image/jpg');

		$("#photo").val(croppedImage);

		$('#user_img').attr('src', croppedImage);


		$('#exampleModalCenter').modal('toggle');

	
	});

	


    $("#ratio43").click(function () {
        var options = {
            aspectRatio: 4 / 3, 
			dragMode: 'move',
			viewMode: 1,
			preview: '.preview'
    };
        $("#canvas").cropper('destroy').cropper(options);
	});

	$("#ratio169").click(function () {
		var options = {
			aspectRatio: 16 / 9,
			dragMode: 'move',
			viewMode: 1,
			preview: '.preview'
		};
		$("#canvas").cropper('destroy').cropper(options);
	});

	$("#ratiofree").click(function () {
		var options = {
			dragMode: 'move',
			viewMode: 1,
			preview: '.preview'
		};
		$("#canvas").cropper('destroy').cropper(options);
	});


	$("#rotateleft").click(function () {
		$("#canvas").cropper("rotate", -30);
	});

	$("#rotateright").click(function () {
		$("#canvas").cropper("rotate", 30);
	});


	$('#showmodal').click(function () {
		
		$('#exampleModalCenter').modal('show')
		
	});

	

	$("#img_file").on('change', function () {

		if (this.files && this.files[0]) {
			var FileSize = this.files[0].size / 1024 / 1024;
			if (this.files[0].type.match(/^image\//) && FileSize<2) {

				
				
				
                var reader = new FileReader();
                reader.onload = function (e) {
					var img = new Image();
					var bool = true;
					img.onload = function () {


						if (img.width < 300) {
							bool = false;
							alert("WRONG SIZE!!!");
						}
						else {
							$('#exampleModalCenter').modal('show');
							$("#exampleModalCenter").on('shown.bs.modal', function () {



								context.canvas.width = img.width;
								context.canvas.height = img.height;
								context.drawImage(img, 0, 0);


								//Раніше написане не відноситься до кропера
								var cropper = $canvas.cropper('destroy').cropper({									
									dragMode: 'move',
									viewMode: 1,
									preview: '.preview',
									rotatable: true
								});
								

							});
						}




                    }

                    img.src = e.target.result;
				}
				reader.readAsDataURL(this.files[0]);
			
            }
            else {
                alert("Invalid file type or too large");
            }
        }
        else {
            //alert("Please select a file.");
			$('#user_img').attr('src', "/Content/default_avatar_community.png");
			$("#photo").val("");
        }
    });
});