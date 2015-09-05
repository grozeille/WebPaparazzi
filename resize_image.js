if (document.body.getElementsByTagName('img').length > 0) {
    var img = document.body.getElementsByTagName('img')[0];
    if (img !== undefined && img !== null) {
        imageWidth = img.width, //need the raw width due to a jquery bug that affects chrome
        imageHeight = img.height, //need the raw height due to a jquery bug that affects chrome
        maxWidth = window.innerWidth,
        maxHeight = window.innerHeight,
        widthRatio = maxWidth / imageWidth,
        heightRatio = maxHeight / imageHeight;

            var ratio = widthRatio; //default to the width ratio until proven wrong

            if (widthRatio * imageHeight > maxHeight) {
                ratio = heightRatio;
            }

            //now resize the image relative to the ratio
            img.width = imageWidth * ratio;
            img.height = imageHeight * ratio;

            //and center the image vertically and horizontally
            img.style.margin = 'auto';
            img.style.position = 'absolute';
            img.style.top = 0;
            img.style.bottom = 0;
            img.style.left = 0;
            img.style.right = 0;
    }
}

