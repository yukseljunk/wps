<?php
$url=htmlspecialchars($_GET["url"]);
$file=htmlspecialchars($_GET["file"]);
$folder=htmlspecialchars($_GET["folder"]);
$thsize=$_GET["thsize"];
$maxsize=$_GET["maxsize"];
if($thsize==""){
 $thsize=200;
}
if($maxsize==""){
  $maxsize=0;
}
if($maxsize=="0"){
  $maxsize=0;
}

//echo 'Url: ' . $url . '<br/>';
//echo 'Filename: ' . $file. '<br/>';
//echo 'Folder: ' . $folder . '<br/>';
//test here

if (!file_exists($folder)) {
    mkdir($folder, 0777, true);
}
$finalFile=$folder.'/'.$file;
file_put_contents($finalFile, file_get_contents($url));
createThumbnail($finalFile,$thsize,$thsize);

if($maxsize>0){
  createThumbnail($finalFile,$maxsize,$maxsize, $finalFile,false);
}

function createThumbnail($filepath, $thumbnail_width, $thumbnail_height, $newfilename="",$echoDimensions=true) {
    list($original_width, $original_height, $original_type) = getimagesize($filepath);
    if($echoDimensions){
        
        if($newfilename==""){
        echo $original_width .'x'. $original_height;
        }
        else
        {
            echo $thumbnail_width .'x'. $thumbnail_height;
        
        }
        
    }
    if ($original_width > $original_height) {
        $new_width = $thumbnail_width;
        $new_height = intval($original_height * $new_width / $original_width);
    } else {
        $new_height = $thumbnail_height;
        $new_width = intval($original_width * $new_height / $original_height);
    }
    $dest_x = intval(($thumbnail_width - $new_width) / 2);
    $dest_y = intval(($thumbnail_height - $new_height) / 2);

    if ($original_type === 1) {
        $imgt = "ImageGIF";
        $imgcreatefrom = "ImageCreateFromGIF";
    } else if ($original_type === 2) {
        $imgt = "ImageJPEG";
        $imgcreatefrom = "ImageCreateFromJPEG";
    } else if ($original_type === 3) {
        $imgt = "ImagePNG";
        $imgcreatefrom = "ImageCreateFromPNG";
    } else {
        return false;
    }

    $old_image = $imgcreatefrom($filepath);
    $new_image = imagecreatetruecolor($thumbnail_width, $thumbnail_height);
    imagecopyresampled($new_image, $old_image, $dest_x, $dest_y, 0, 0, $new_width, $new_height, $original_width, $original_height);

    $file_name = basename($filepath);/* Name of the Image File*/
    $ext   = pathinfo($file_name, PATHINFO_EXTENSION);
    
    /* Adding image name _thumb for thumbnail image */
    $file_name = dirname($filepath) .'/'. basename($file_name, ".$ext") . '-'.$thumbnail_width.'x'.$thumbnail_height.'.' . $ext;
    if($newfilename==""){
       $imgt($new_image, $file_name);
       return file_exists($file_name);
    }
    $imgt($new_image, $newfilename);
    return file_exists($newfilename);
}
?>

