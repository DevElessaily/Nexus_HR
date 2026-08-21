// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// ---------------------------------------------------------------------------
// Profile photo lightbox: opens an employee's profile image full-size when
// its avatar is clicked. Used on Employee Details (and anywhere else an
// <img> avatar exists). See the .nx-lightbox markup in _Layout.cshtml.
// ---------------------------------------------------------------------------
function openLightbox(src) {
    if (!src) return;
    var img = document.getElementById('nxLightboxImg');
    var box = document.getElementById('nxLightbox');
    if (!img || !box) return;
    img.src = src;
    box.classList.add('nx-open');
    document.body.style.overflow = 'hidden';
}

function closeLightbox() {
    var box = document.getElementById('nxLightbox');
    if (!box) return;
    box.classList.remove('nx-open');
    document.body.style.overflow = '';
}

document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape') closeLightbox();
});
