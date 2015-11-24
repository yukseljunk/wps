<?php
set_time_limit(600);
error_reporting(E_ALL & ~E_NOTICE); 
include("wp-config.php");
?>
<html><head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>BOT</title>
<style>
	body {font:12px/19px Myriad Pro; margin:auto; background:url('bot.jpg') #f5f5f5; background-repeat:no-repeat; background-attachment:fixed; background-position:right top;}
	table,tr,td,h1,h2,h3,h4,h5,h6,ul,li,form,textarea {padding:0; margin:0; list-style-type:none; border:none;}
	img {border:none;}
	a {text-decoration:none;}
	:focus {outline:none}
	.genel { width:800px; border:1px solid #000; background:#eee; border-radius:4px; margin:0 auto;}
	.genel h2 { text-align:center; margin:10px; padding:3px; }
	.genel .anaform input, .anabaslik input, .anabaslik select, .anabaslik textarea { border-radius:4px; border:1px solid #7f8520; padding:4px; font-weight:bold; background:#d5d5d5; }
	.btn { width:150px; padding:2px; border-radius:3px; border:1px solid #000; margin-bottom:5px; margin-top:5px;}
	.anabaslik {font-size:14px; text-align:center; padding:5px; border-bottom:1px solid #7f8520;}
	.ekle {width:100px; color:#983541; border:1px solid #983541; margin-bottom:10px;}
	.hata {background-color:#e88d8d;}
</style>
</head>
<body>
<div style="margin:10px;">
<div class="genel">
<center>
<?php
function yaz($a){
echo "<pre>";
print_r($a);
echo "</pre>";
}

function cek($url){
  $curl = curl_init();
  // Setup headers - I used the same headers from Firefox version 2.0.0.6
  // below was split up because php.net said the line was too long. :/
  $header[0] = "Accept: text/xml,application/xml,application/xhtml+xml,";
  $header[0] .= "text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
  $header[] = "Cache-Control: max-age=0";
  $header[] = "Connection: keep-alive";
  $header[] = "Keep-Alive: 300";
  $header[] = "Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.7";
  $header[] = "Accept-Language: en-us,en;q=0.5";
  $header[] = "Pragma: "; // browsers keep this blank.
  curl_setopt($curl, CURLOPT_URL, $url);
  curl_setopt($curl, CURLOPT_USERAGENT, 'Mozilla/5.0 (Windows NT 6.2; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0');
  curl_setopt($curl, CURLOPT_HTTPHEADER, $header);
  curl_setopt($curl, CURLOPT_COOKIEFILE, dirname(__FILE__). '/cookie.txt'); //read cookies from here
  curl_setopt($curl, CURLOPT_COOKIEJAR, dirname(__FILE__) . '/cookie.txt'); //save cookies here
  curl_setopt($curl, CURLOPT_REFERER, 'http://www.google.com');
  curl_setopt($curl, CURLOPT_HEADER, 1);
  curl_setopt($curl, CURLOPT_SSL_VERIFYHOST, false);
  curl_setopt($curl, CURLOPT_SSL_VERIFYPEER, false);
  curl_setopt($curl, CURLOPT_FOLLOWLOCATION, false);
  curl_setopt($curl, CURLOPT_MAXREDIRS, 10); /* Max redirection to follow */
  curl_setopt($curl, CURLOPT_ENCODING, 'gzip,deflate');
  curl_setopt($curl, CURLOPT_AUTOREFERER, true);
  curl_setopt($curl, CURLOPT_RETURNTRANSFER, 1);
  curl_setopt($curl, CURLOPT_TIMEOUT, 30);
  $html = curl_exec($curl); // execute the curl command
  curl_close($curl); // close the connection
  return $html; // and finally, return $html
}

function seo_url($w){
		$w = preg_replace("@[^a-z0-9\öşıüğçİŞĞÜÖÇ ]+@i","",$w);
        $tr = array("ı","Ğ","ğ","Ü","ü","Ş","ş","İ","Ö","ö","Ç","ç","&","<",">","+"," ");
        $en = array("i","g","g","u","u","s","s","i","o","o","c","c","","","","","-");
        $w = str_replace($tr,$en,$w);
        return @strtolower($w);} 

function resimekle($url,$isim,$sira){
global $pid;
$url = html_entity_decode($url);
$savepath = 'wp-content/uploads/';
if(strlen(seo_url($isim)) > 50){ 
$sade = substr(seo_url($isim),0,50);
}else{
$sade = seo_url($isim);}
$fullfilename = $sade.'-'.$sira.'.jpg';

if(function_exists('curl_init')){
$fp = fopen($savepath.$fullfilename,'w+');
$ch = curl_init();
curl_setopt($ch , CURLOPT_URL, $url);
curl_setopt($ch, CURLOPT_REFERER, 'http://google.com');
curl_setopt($ch , CURLOPT_USERAGENT, 'firefox/2.0.11');
curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, false);
curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
curl_setopt($ch , CURLOPT_FILE, $fp);
curl_exec($ch);
curl_close($ch);
fclose($fp);
}else{
copy($url,$savepath.$fullfilename);} 

$fullfilename0 = $savepath.$fullfilename;
$wp_filetype = wp_check_filetype(basename($fullfilename0), null );
  $wp_upload_dir = wp_upload_dir();
  $attachment = array(
     'guid' => $wp_upload_dir['baseurl'] . _wp_relative_upload_path( $fullfilename0 ), 
     'post_mime_type' => $wp_filetype['type'],
     'post_title' => preg_replace('/\.[^.]+$/', '', $isim.' - '.$sira),
     'post_content' => preg_replace('/\.[^.]+$/', '', $paragraf),
     'post_status' => 'inherit'
  );
  $attach_id = wp_insert_attachment( $attachment, $fullfilename0, $pid);
  require_once(ABSPATH . 'wp-admin/includes/image.php');
  $attach_data = wp_generate_attachment_metadata( $attach_id, $fullfilename0 );
  wp_update_attachment_metadata( $attach_id, $attach_data );

$don[0] = $attach_id;
$don[1] = get_bloginfo('url').'/'.$fullfilename0;
return $don;}

$kategori = $_GET['k'];
$sayfa = $_GET['s'] ? $_GET['s']:1;
?>
<h2>BOT</h2><br />
<form action="" method="get" class="anaform">
<table width="100%" border="1" style="padding:10px;">
  <tr>
    <td align="center"><strong>Adres</strong></td>
    <td align="center"><strong>Sayfa Numarası</strong></td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="center"><input type="text" size="50" name="k" style="text-align:center;" value="<?php echo $kategori; ?>"></td>
    <td align="center"><input type="text" size="10" name="s" style="text-align:center;" value="<?php echo $sayfa; ?>"></td>
    <td align="center"><input type="submit" value="Çek" class="btn" /></td>
  </tr>
</table>
</form>
<?php
if(!empty($kategori)){

$git = cek(str_replace(" ","+",$kategori).'&page='.$sayfa);

preg_match_all('#<div class="listing-title clearfix">(.*?)<a href="(.*?)" title="(.*?)"#si',$git,$bak);
if(empty($bak[2][0])){
preg_match_all('#<div class="titl(.*?)"><a href="(.*?)" title="(.*?)"#si',$git,$bak);}

for($i = 0 ; $i < count($bak[0]); $i++){

preg_match('#/listing/(.*?)/#si',$bak[2][$i],$id);
$baslik = addslashes($bak[3][$i]);
$kontrol = $id[1];
$query = "select meta_id from wp_postmeta WHERE meta_value='$kontrol'";
$result = mysql_fetch_row(mysql_query($query));
if(empty($result[0])){

if(!preg_match('/etsy.com/i', $bak[2][$i])){
$bak[2][$i] = 'https://www.etsy.com'.$bak[2][$i];}
if(!preg_match('/http/i', $bak[2][$i])){
$bak[2][$i] = 'https:'.$bak[2][$i];}
$urune_git = cek($bak[2][$i]); 

preg_match('#<meta name="description" content="(.*?)"#si',$urune_git,$meta_description);

preg_match('#<h2>Related to this Item</h2>(.*?)</ul>#si',$urune_git,$tag_ayir);
preg_match_all('#<a href="(.*?)">(.*?)</a>#si',$tag_ayir[1],$etiketler);
preg_match_all('#data-full-image-href="(.*?)"#si',$urune_git,$resimler);
preg_match('#<meta itemprop="price" content="(.*?)"#si',$urune_git,$price);

preg_match('#<div id="description-text">(.*?)</div>#si',$urune_git,$icerik);
$icerik[1] = preg_replace('#<a.*?>.*?</a>#i', '', $icerik[1]);
$icerik[1] = strip_tags($icerik[1],'<table></table><tr></tr><th></th><td></td></ul><ul><br><strong></strong><li></li><p></p>');

$etikets = implode(",", $etiketler[2]);

kses_remove_filters(); 
$insert_post = array();
$insert_post['post_title'] = $baslik;
$insert_post['tags_input'] = $etikets;
$insert_post['post_status'] = 'draft';
$insert_post['post_author'] = 1;
$insert_post['post_category'] = array($ekle);
$pid = wp_insert_post($insert_post);

add_post_meta($pid,'kontrol',$kontrol);
add_post_meta($pid, '_aioseop_title', $baslik);
add_post_meta($pid, '_aioseop_description', $meta_description[1]);
add_post_meta($pid, '_aioseop_keywords', $etikets);

$post_content = '';		
if(!empty($resimler[1][0])){		
$post_content .= '<div style="width: 300px; margin-right: 10px;">';
for($a = 0 ; $a < count($resimler[0]); $a++){
$image = resimekle($resimler[1][$a],$baslik,$a);
if($a == '0'){
add_post_meta($pid,'_thumbnail_id',$image[0]);}
$post_content .= '<div style="width: 70px; float: left; margin-right: 15px; margin-bottom: 3px;"><a href="'.$image[1].'"><img src="'.$image[1].'" alt="'.$baslik.'" width="70px" height="70px" title="'.$baslik.'" /></a></div>';}
$post_content .= '</div><h4>Price: $'.$price[1].'</h4>';
}else{
$post_content .= '<h4>Price: $'.$price[1].'</h4>';}
$post_content .= '<strong>Description: </strong>' . $icerik[1];

$my_post = array();
$my_post['ID'] = $pid;
$my_post['post_content'] = $post_content;
wp_update_post( $my_post );

echo '<strong>'.$baslik.'</strong> Eklendi.<br>';
}else{
echo '<strong>'.$baslik.'</strong> Daha Once Eklenmis.<br>';}



flush();
ob_get_contents();
ob_flush();

}
}
?>
</center>
</div>
</div>
</body>
</html>