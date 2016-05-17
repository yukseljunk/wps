<?php
error_reporting( E_ALL );
ini_set('display_startup_errors',1); 
ini_set("display_errors", 1);
$file=htmlspecialchars($_GET["file"]);

  $zip = new ZipArchive;
  $res = $zip->open($file);
  if ($res === TRUE) {
    $zip->extractTo('wp-content/plugins/');
    $zip->close();
    echo 'OK';
  } else {
    echo 'Problem';
  }
 

echo error_get_last() 
?>