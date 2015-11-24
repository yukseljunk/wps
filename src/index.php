<?php
function yaz($a){echo "<pre>";print_r($a);echo "</pre>";}
function cek($url){  $curl = curl_init(); curl_setopt($curl, CURLOPT_URL, $url);  $html = curl_exec($curl); return $html; }
function seo_url($w){$w = preg_replace("@[^a-z0-9\öşıüğçİŞĞÜÖÇ ]+@i","",$w);$tr = array("ı","Ğ","ğ","Ü","ü","Ş","ş","İ","Ö","ö","Ç","ç","&","<",">","+"," ");$en = array("i","g","g","u","u","s","s","i","o","o","c","c","","","","","-");$w = str_replace($tr,$en,$w);return @strtolower($w);} 

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
<form action="" method="get" class="anaform">
    <input type="text" size="50" name="k" style="text-align:center;" value="<?php echo $kategori; ?>">
    <input type="text" size="10" name="s" style="text-align:center;" value="<?php echo $sayfa; ?>">
</form>
<?php
if(empty($kategori)) return;

$git = cek(str_replace(" ","+",$kategori).'&page='.$sayfa);
preg_match_all('#<div class="listing-title clearfix">(.*?)<a href="(.*?)" title="(.*?)"#si',$git,$bak);
if(empty($bak[2][0])){
	preg_match_all('#<div class="titl(.*?)"><a href="(.*?)" title="(.*?)"#si',$git,$bak);
}

for($i = 0 ; $i < count($bak[0]); $i++){
	preg_match('#/listing/(.*?)/#si',$bak[2][$i],$id);
	$baslik = addslashes($bak[3][$i]);
	$kontrol = $id[1];
	$query = "select meta_id from wp_postmeta WHERE meta_value='$kontrol'";
	$result = mysql_fetch_row(mysql_query($query));
	
	if(!empty($result[0])){
		echo '<strong>'.$baslik.'</strong> Daha Once Eklenmis.<br>';
		continue;
	}
	
	if(!preg_match('/etsy.com/i', $bak[2][$i])){
		$bak[2][$i] = 'https://www.etsy.com'.$bak[2][$i];
	}
	if(!preg_match('/http/i', $bak[2][$i])){
		$bak[2][$i] = 'https:'.$bak[2][$i];
	}
	
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
				add_post_meta($pid,'_thumbnail_id',$image[0]);
			}
			$post_content .= '<div style="width: 70px; float: left; margin-right: 15px; margin-bottom: 3px;"><a href="'.$image[1].'"><img src="'.$image[1].'" alt="'.$baslik.'" width="70px" height="70px" title="'.$baslik.'" /></a></div>';
		}
		$post_content .= '</div><h4>Price: $'.$price[1].'</h4>';
	}
	else{
		$post_content .= '<h4>Price: $'.$price[1].'</h4>';
	}

	$post_content .= '<strong>Description: </strong>' . $icerik[1];

	$my_post = array();
	$my_post['ID'] = $pid;
	$my_post['post_content'] = $post_content;
	wp_update_post( $my_post );

	echo '<strong>'.$baslik.'</strong> Eklendi.<br>';

	
	flush();
	ob_get_contents();
	ob_flush();

}

?>