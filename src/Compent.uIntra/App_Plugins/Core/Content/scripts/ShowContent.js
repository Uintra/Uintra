function showContent() {
    var wrapper = document.getElementById('wrapper');
    var isLoading = wrapper.classList.contains('_loading');

    if (wrapper != null && isLoading) {
        wrapper.classList.remove('_loading');
    }
}

export default showContent;