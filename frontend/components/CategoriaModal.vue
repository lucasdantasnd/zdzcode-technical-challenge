<template>
  <div v-if="show" class="modal-overlay" @click.self="close">
    <div class="modal-content">
      <div class="modal-header">
        <h3 class="modal-title">{{ isEdit ? 'Editar Categoria' : 'Nova Categoria' }}</h3>
        <button class="modal-close" @click="close">&times;</button>
      </div>

      <form @submit.prevent="handleSubmit">
        <div class="form-group">
          <label class="form-label" for="nome">Nome</label>
          <input
            id="nome"
            v-model="form.nome"
            type="text"
            class="form-input"
            :class="{ 'invalid': nameTouched && !isNameValid }"
            placeholder="Mínimo de 5 caracteres"
            @blur="nameTouched = true"
          />
          <span v-if="nameTouched && !isNameValid" class="error-message">
            O nome é obrigatório e deve ter pelo menos 5 caracteres.
          </span>
        </div>

        <div class="form-group">
          <label class="form-label" for="descricao">Descrição</label>
          <textarea
            id="descricao"
            v-model="form.descricao"
            class="form-input"
            style="resize: vertical; min-height: 100px;"
            placeholder="Descrição opcional da categoria"
          ></textarea>
        </div>

        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" @click="close">Cancelar</button>
          <button
            type="submit"
            class="btn btn-primary"
            :disabled="!isFormValid || submitting"
          >
            {{ submitting ? 'Salvando...' : 'Salvar Categoria' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { reactive, computed, watch, ref } from 'vue'

const props = defineProps({
  show: {
    type: Boolean,
    required: true
  },
  categoria: {
    type: Object,
    default: null
  }
})

const emit = defineEmits(['close', 'save'])

const isEdit = computed(() => !!props.categoria)
const submitting = ref(false)
const nameTouched = ref(false)

const form = reactive({
  nome: '',
  descricao: ''
})

// Validation logic (AC 04)
const isNameValid = computed(() => form.nome && form.nome.trim().length >= 5)
const isFormValid = computed(() => isNameValid.value)

// Reset form and populate when opening/changing category
watch(() => props.show, (newVal) => {
  if (newVal) {
    nameTouched.value = false
    if (props.categoria) {
      form.nome = props.categoria.nome
      form.descricao = props.categoria.descricao || ''
    } else {
      form.nome = ''
      form.descricao = ''
    }
  }
})

const close = () => {
  emit('close')
}

const handleSubmit = async () => {
  if (!isFormValid.value) return
  submitting.value = true
  
  const success = await emit('save', {
    id: props.categoria?.id,
    nome: form.nome.trim(),
    descricao: form.descricao?.trim() || null
  })

  submitting.value = false
  if (success) {
    close()
  }
}
</script>
