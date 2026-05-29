<script setup>
// Redirect root path to products catalog
definePageMeta({
  middleware: [
    function (to, from) {
      return navigateTo('/produtos')
    }
  ]
})
</script>
