<?php
/**
 * Plugin Name: External Link
 * Plugin URI: http://www.nalgorithm.com
 * Description: This plugin adds a metadata link value to be injected to template.
 * Version: 1.0.0
 * Author: Yuksel Daskin
 * Author URI: http://www.nalgorithm.com
 * License: Commercial
 */

add_action('admin_menu', 'external_link_menu');

function external_link_menu() {
	add_submenu_page('options-general.php','External Link Settings', 'ExternalLink', 'administrator', 'external-link-settings', 'external_link_settings_page', 'dashicons-admin-generic');
}

function external_link_settings_page() {
  // 
  ?>
  <div class="wrap">
<h2>ExternalLink Settings</h2>

<form method="post" action="options.php">
    <?php settings_fields( 'external-link-settings-group' ); ?>
    <?php do_settings_sections( 'external-link-settings-group' ); ?>
    <table class="form-table">
        <tr valign="top">
        <th scope="row">External Link Html</th>
        <td><textarea rows="5" cols="100" name="external_link_html"><?php echo esc_attr( get_option('external_link_html') ); ?></textarea></td>
        </tr>
    </table>
    
    <?php submit_button(); ?>

</form>
</div>
  <?php
}
add_action( 'admin_init', 'external_link_settings' );

function external_link_settings() {
	register_setting( 'external-link-settings-group', 'external_link_html' );
}

?>