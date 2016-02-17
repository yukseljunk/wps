<?php
/**
 * Plugin Name: Bounce Popup
 * Plugin URI: http://www.nalgorithm.com
 * Description: This plugin adds a popup when visitor clicks back button or backspace to leave the site.
 * Version: 1.0.0
 * Author: Yuksel Daskin
 * Author URI: http://www.nalgorithm.com
 * License: Commercial
 */
add_action( 'wp_head', 'bounce_popup' );
function bounce_popup() {
	
	$urls = explode("\n", get_option('bouncepopup_url'));
	$randomIndex = mt_rand(0, count($urls) -1)
	
  ?>
 
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
  <script type="text/javascript" src="<?php echo plugins_url();?>/BouncePopup/fancybox/jquery.fancybox.pack.js?v=2.1.5"></script>
  <link rel="stylesheet" href="<?php echo plugins_url();?>/BouncePopup/fancybox/jquery.fancybox.css?v=2.1.5" type="text/css" media="screen" />

	<script type="text/javascript">
		jQuery(document).ready(function($) {

  if (window.history && window.history.pushState) {

    window.history.pushState('forward', null, './#forward');

     $(window).on('popstate', function() {
		 var showPopup=true;
		 if(!document.referrer){
						 
		 }
		 else{
			 if(window.location.host==document.referrer.split('/')[2]){
				 showPopup=false;
			 }
			 else{
				

			 }
		 }
		 if( showPopup){
			
   			    $.fancybox({
				 'width'				: '75%',
							'height'			: '75%',
							'autoScale'			: false,
							'transitionIn'		: 'none',
							'transitionOut'		: 'none',
							'type'				: 'iframe',
							'href'				:'<?php echo $urls[$randomIndex]; ?>'
			  });
					  
		 }
    });
  }
});
	</script>

  <?php
}
add_action('admin_menu', 'bounce_popup_menu');

function bounce_popup_menu() {
	add_submenu_page('options-general.php','BouncePopup Settings', 'BouncePopup', 'administrator', 'bounce-popup-settings', 'bounce_popup_settings_page', 'dashicons-admin-generic');
}

function bounce_popup_settings_page() {
  // 
  ?>
  <div class="wrap">
<h2>BouncePopup Settings</h2>

<form method="post" action="options.php">
    <?php settings_fields( 'bounce-popup-settings-group' ); ?>
    <?php do_settings_sections( 'bounce-popup-settings-group' ); ?>
    <table class="form-table">
        <tr valign="top">
        <th scope="row">Urls to choose randomly</th>
        <td><textarea rows="5" cols="100" name="bouncepopup_url"><?php echo esc_attr( get_option('bouncepopup_url') ); ?></textarea></td>
        </tr>
    </table>
    
    <?php submit_button(); ?>

</form>
</div>
  <?php
}
add_action( 'admin_init', 'bounce_popup_settings' );

function bounce_popup_settings() {
	register_setting( 'bounce-popup-settings-group', 'bouncepopup_url' );
}

?>