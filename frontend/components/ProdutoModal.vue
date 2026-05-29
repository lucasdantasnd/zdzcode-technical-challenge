<template>
  <div v-if="show" class="modal-overlay" @click.self="close">
    <div class="modal-content">
      <div class="modal-header">
        <h3 class="modal-title">{{ isEdit ? 'Editar Produto' : 'Novo Produto' }}</h3>
        <button class="modal-close" @click="close">&times;</button>
      </div>

      <form @submit.prevent="handleSubmit">
        <div class="form-group">
          <label class="form-label" for="prod-nome">Nome</label>
          <input
            id="prod-nome"
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
          <label class="form-label" for="prod-preco">Preço (R$)</label>
          <input
            id="prod-preco"
            v-model.number="form.preco"
            type="number"
            step="0.01"
            min="0.01"
            class="form-input"
            :class="{ 'invalid': precoTouched && !isPrecoValid }"
            placeholder="Ex: 99.90"
            @blur="precoTouched = true"
          />
          <span v-if="precoTouched && !isPrecoValid" class="error-message">
            O preço é obrigatório e deve ser maior que zero.
          </span>
        </div>

        <div class="form-group">
          <label class="form-label" for="prod-categoria">Categoria</label>
          <select
            id="prod-categoria"
            v-model="form.categoriaId"
            class="form-input"
            :class="{ 'invalid': categoriaTouched && !isCategoriaValid }"
            @blur="categoriaTouched = true"
          >
            <option value="" disabled>Selecione uma categoria...</option>
            <option
              v-for="cat in categorias"
              :key="cat.id"
              :value="cat.id"
            >
              {{ cat.nome }}
            </option>
          </select>
          <span v-if="categoriaTouched && !isCategoriaValid" class="error-message">
            Selecione uma categoria válida para o produto.
          </span>
        </div>

        <div class="form-group">
          <label class="form-label" for="prod-desc">Descrição</label>
          <textarea
            id="prod-desc"
            v-model="form.descricao"
            class="form-input"
            style="resize: vertical; min-height: 80px;"
            placeholder="Descrição opcional do produto"
          ></textarea>
        </div>

        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" @click="close">Cancelar</button>
          <button
            type="submit"
            class="btn btn-primary"
            :disabled="!isFormValid || submitting"
          >
            {{ submitting ? 'Salvando...' : 'Salvar Produto' }}
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
  produto: {
    type: Object,
    default: null
  },
  categorias: {
    type: Array,
    required: true
  }
})

const emit = defineEmits(['close', 'save'])

const isEdit = computed(() => !!props.produto)
const submitting = ref(false)

const nameTouched = ref(false)
const precoTouched = ref(false)
const categoriaTouched = ref(false)

const form = reactive({
  nome: '',
  preco: '',
  categoriaId: '',
  descricao: ''
})

// Validation rules (AC 04)
const isNameValid = computed(() => form.nome && form.nome.trim().length >= 5)
const isPrecoValid = computed(() => form.preco !== null && form.preco !== undefined && form.preco > 0)
const isCategoriaValid = computed(() => !!form.categoriaId)
const isFormValid = computed(() => isNameValid.value && isPrecoValid.value && isCategoriaValid.value)

// Reset form and populate when opening/changing product
watch(() => props.show, (newVal) => {
  if (newVal) {
    nameTouched.value = false
    precoTouched.value = false
    categoriaTouched.value = false
    
    if (props.produto) {
      form.nome = props.produto.nome
      form.preco = props.produto.preco
      form.categoriaId = props.produto.categoriaId
      form.descricao = props.produto.descricao || ''
    } else {
      form.nome = ''
      form.preco = ''
      form.categoriaId = ''
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
    id: props.produto?.id,
    nome: form.nome.trim(),
    preco: parseFloat(form.preco),
    categoriaId: parseInt(form.categoriaId, 10),
    descricao: form.descricao?.trim() || null
  })

  submitting.value = false
  if (success) {
    close()
  }
}
</script>
