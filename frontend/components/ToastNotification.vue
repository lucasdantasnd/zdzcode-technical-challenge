<template>
  <div class="toast-container">
    <div
      v-for="toast in toasts"
      :key="toast.id"
      class="toast"
      :class="`toast-${toast.type}`"
    >
      <div class="toast-content">
        <div class="toast-title">{{ toast.title }}</div>
        <div class="toast-text">{{ toast.message }}</div>
      </div>
      <button class="modal-close" @click="removeToast(toast.id)">&times;</button>
    </div>
  </div>
</template>

<script setup>
import { useToasts } from '~/composables/useToasts'

const { toasts, removeToast } = useToasts()
</script>
