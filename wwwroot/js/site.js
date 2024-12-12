// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// scripts.js
document.addEventListener('DOMContentLoaded', () => {
    const scrollContainer = document.querySelector('.photo-scroll');
    const photos = Array.from(scrollContainer.children);

    // Dupliceer de foto's
    photos.forEach(photo => {
        const clone = photo.cloneNode(true);
        scrollContainer.appendChild(clone);
    });
    const photoScroll = document.querySelector('.photo-scroll');
    const imageCount = photoScroll.children.length / 2; // Aantal unieke foto's
    const duration = imageCount * 5; // Stel duur per foto in
    photoScroll.style.animationDuration = `${duration}s`;
});
