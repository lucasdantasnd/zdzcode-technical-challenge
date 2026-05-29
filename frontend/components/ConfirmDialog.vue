<template>
  <div v-if="show" class="modal-overlay" @click.self="cancel">
    <div class="modal-content">
      <div class="modal-header">
        <h3 class="modal-title">{{ title }}</h3>
        <button class="modal-close" @click="cancel">&times;</button>
      </div>
      
      <div class="form-group" style="margin-bottom: 2rem;">
        <p style="color: var(--text-muted); line-height: 1.5;">{{ message }}</p>
      </div>

      <div class="modal-footer">
        <button class="btn btn-secondary" @click="cancel">Cancelar</button>
        <button class="btn btn-danger" @click="confirm">Confirmar Exclusão</button>
      </div>
    </div>
  </div>
</template>

<script setup>
defineProps({
  show: {
    type: Boolean,
    required: true
  },
  title: {
    type: String,
    default: 'Confirmar Ação'
  },
  message: {
    type: String,
    default: 'Deseja realmente prosseguir com esta ação?'
  }
})

const emit = defineEmits(['confirm', 'cancel'])

const confirm = () => {
  emit('confirm')
}

const cancel = () => {
  emit('cancel')
}
</script>
