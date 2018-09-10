$(function () {
    var $canvas = $("#canvas");
    context = $canvas.get(0).getContext('2d');
    var gallery = function () {
         
        function init() {

            $(".uploadimage").on("click", function () {
                var inputFile = $("<input/>").attr('type', 'file')
                    .attr('name', 'chosenImage')
					.attr('style', 'display:none')
					.attr('accept','.jpg,.jpeg,.png');
                $("#fileContainer").append(
                    inputFile
                );

                inputFile.click();

                inputFile.change(function () {
                    if (this.files && this.files[0]) {
                        if (this.files[0].type.match(/^image\//)) {
                            loadImage(this.files[0]);
                        }
                        else {
                            bootbox.alert("Invalid type");
                        }

                    }
                    else {
                        bootbox.alert("Please select an image");
                    }
                });



            });


        }

        function loadImage(fileImage) {
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
                                aspectRatio: 9 / 9,
                                viewMode: 1,
                                preview: '.preview',
                                rotatable: true,
                                crop: function (e) {
                                    var data = e.detail;
                                    var w = Math.round(data.width);
                                    if (w < 250)
                                        this.cropper.setData({ width: 250 });
                                }
                                //minCropBoxWidth: 250,
                            });


                        });
                    }




                }

                img.src = e.target.result;
            }
            reader.readAsDataURL(fileImage);

        }

        function run() {
            init();
        }
        return { run: run };
    }();
    gallery.run();


    $("#rotateleft").click(function () {
        $("#canvas").cropper("rotate", -30);
    });

    $("#rotateright").click(function () {
        $("#canvas").cropper("rotate", 30);
    });

    $("#saveServer").click(function () {

        var croppedImage = $canvas.cropper('getCroppedCanvas').toDataURL('image/jpg');

        var div = $('#listphotos');
        var data = '<div class="col-md-2">';
        data += '<div class="thumbnail">';
        data += '<i class="fa fa-times fa-2x icon-delete" aria-hide="true" id="lol" style="display:block"></i>';
        data += '<img src="'+croppedImage+'" />';
        data += '</div>';
        data += '</div>';
        div.append(data);

        //$('#user_img').attr('src', croppedImage);


        $('#exampleModalCenter').modal('toggle');

	});

	$(document).on('click', '.icon-delete', function () {
		$(this).parent().parent().remove();

	});

	
	
});