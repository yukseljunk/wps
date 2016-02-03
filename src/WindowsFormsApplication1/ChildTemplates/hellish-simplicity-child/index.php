<?php
/**
 * The main template file.
 *
 * @package Hellish Simplicity
 * @since Hellish Simplicity 1.1
 */

get_header(); ?>

<div id="content-area">
	<div id="site-content" role="main"><?php

// If on search page, then display what we searched for
if ( is_search() ) { ?>
		<h1 class="page-title">
			<?php printf( __( 'Search Results for: "%s" ...', 'hellish-simplicity' ), get_search_query() ); ?>
		</h1><?php
}

// Load main loop
if ( have_posts() ) {

	// Start of the Loop
	while ( have_posts() ) {
		the_post();
		?>

		<article id="post-<?php the_ID(); ?>" <?php post_class(); ?>>

			<header class="entry-header">
				<h1 class="entry-title">
          <?php

					// Don't display links on singular post titles
					if ( is_singular() ) {
            $this_page_id = get_the_ID(); 
            $fk=get_post_meta($this_page_id,'_aioseop_title',true);
			$spacePos=strpos($fk, ' ', 60);
            echo mb_substr($fk,0,$spacePos);          
					} else {
						?>
						<a href="<?php the_permalink(); ?>" title="<?php echo esc_attr( sprintf( __( 'Permalink to %s', 'hellish-simplicity' ), the_title_attribute( 'echo=0' ) ) ); ?>" rel="bookmark"><?php $this_page_id = get_the_ID(); $fk=get_post_meta($this_page_id,'_aioseop_title',true);$spacePos=strpos($fk, ' ', 60);echo mb_substr($fk,0,$spacePos); ?></a><?php
					}

					?></h1>
			</header><!-- .entry-header -->

			<div class="entry-content"><?php

				// Display full content for home page and single post pages
				if ( is_home() || is_single() || is_page() ) {
					the_content( __( 'Continue reading <span class="meta-nav">&rarr;</span>', 'hellish-simplicity' ) );
					wp_link_pages( array( 'before' => '<div class="page-links">' . __( 'Pages:', 'hellish-simplicity' ), 'after' => '</div>' ) );
				} else {

					// Use the built in thumbnail system, otherwise attempt to display the latest attachment
					if ( has_post_thumbnail() ) {
						the_post_thumbnail( 'excerpt-thumb' );
					} elseif ( function_exists( 'get_the_image' ) ) {
						get_the_image( array( 'size' => 'thumbnail' ) );
					}
					the_excerpt();
				}
				?>
			</div><!-- .entry-content --><?php

			// Don't display meta information on static pages
			if ( ! is_page() ) { ?>
			<footer class="entry-meta">
				<?php
				printf(
					__( 'Posted on <a href="%1$s" title="%2$s" rel="bookmark"><time class="entry-date" datetime="%3$s">%4$s</time></a><span class="byline"> by <span class="author vcard"><a class="url fn n" href="%5$s" title="%6$s" rel="author">%7$s</a></span></span>', 'hellish-simplicity' ),
					esc_url( get_permalink() ),
					esc_attr( get_the_time() ),
					esc_attr( get_the_date( 'c' ) ),
					esc_html( get_the_date() ),
					esc_url( get_author_posts_url( get_the_author_meta( 'ID' ) ) ),
					esc_attr( sprintf( __( 'View all posts by %s', 'hellish-simplicity' ), get_the_author() ) ),
					get_the_author()
				);

				// Category listings (only display when we have more than one category)
				$categories_list = get_the_category_list( __( ', ', 'hellish-simplicity' ) );
				$all_categories = get_categories();
				if ( 1 < count( $all_categories ) && $categories_list ) {
					?>
					<span class="cat-links">
						<?php printf( __( ' in %1$s', 'hellish-simplicity' ), $categories_list ); ?>
					</span><?php
				}

				// Tag listings
				$tags_list = get_the_tag_list( '', __( ', ', 'hellish-simplicity' ) );
				if ( $tags_list ) {
				?>
				<span class="sep"> | </span>
				<span class="tags-links">
					<?php printf( __( 'Tagged %1$s', 'hellish-simplicity' ), $tags_list ); ?>
				</span><?php
				}

				// Comments info.
				if ( ! post_password_required() && ( comments_open() || '0' != get_comments_number() ) ) { ?>
				<span class="sep"> | </span>
				<span class="comments-link"><?php comments_popup_link( __( 'Leave a comment', 'hellish-simplicity' ), __( '1 Comment', 'hellish-simplicity' ), __( '% Comments', 'hellish-simplicity' ) ); ?></span><?php
				}

				// Edit link
				edit_post_link( __( 'Edit', 'hellish-simplicity' ), '<span class="sep"> | </span><span class="edit-link">', '</span>' );
				?>
			</footer><!-- .entry-meta --><?php
			} ?>

		</article><!-- #post-<?php the_ID(); ?> --><?php

		// If comments are open or we have at least one comment, load up the comment template
		if ( comments_open() || '0' != get_comments_number() ) {
			comments_template( '', true );
		}

	}

	get_template_part( 'template-parts/numeric-pagination' );

}
else {
	get_template_part( 'template-parts/no-results' );
}
?>

	</div><!-- #site-content -->
	<?php get_sidebar(); ?>
</div><!-- #content-area -->

<?php get_footer(); ?>