<template>
  <div>
    <!-- Top Actions -->
    <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 2rem;">
      <h2 style="font-size: 1.25rem; font-weight: 600; color: var(--text-primary);">
        Todas as Categorias ({{ categorias.length }})
      </h2>
      <button class="btn btn-primary" @click="openCreateModal">
        <span style="font-size: 1.1rem; line-height: 1;">+</span>
        Nova Categoria
      </button>
    </div>

    <!-- Loading State -->
    <div v-if="loading" style="display: flex; justify-content: center; padding: 4rem;">
      <div style="color: var(--text-muted); font-weight: 500;">Carregando categorias...</div>
    </div>

    <!-- Empty State -->
    <div v-else-if="categorias.length === 0" class="card empty-state">
      <div class="empty-icon">📂</div>
      <h3>Nenhuma categoria cadastrada</h3>
      <p>Cadastre categorias para organizar seus produtos no catálogo.</p>
      <button class="btn btn-primary" style="margin-top: 1.5rem;" @click="openCreateModal">
        Cadastrar Primeira Categoria
      </button>
    </div>

    <!-- Data Table Grid -->
    <div v-else class="grid-container">
      <table class="data-table">
        <thead>
          <tr>
            <th style="width: 80px;">ID</th>
            <th>Nome</th>
            <th>Descrição</th>
            <th style="width: 150px; text-align: right;">Ações</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="cat in categorias" :key="cat.id">
            <td style="font-family: var(--font-title); font-weight: 600; color: var(--text-muted);">
              #{{ cat.id }}
            </td>
            <td style="font-weight: 600; color: var(--text-primary);">
              {{ cat.nome }}
            </td>
            <td style="color: var(--text-muted); max-width: 300px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap;">
              {{ cat.descricao || '—' }}
            </td>
            <td style="text-align: right;">
              <div class="actions-cell" style="justify-content: flex-end;">
                <button
                  class="btn btn-secondary btn-icon"
                  title="Editar Categoria"
                  @click="openEditModal(cat)"
                >
                  ✏️
                </button>
                <button
                  class="btn btn-danger btn-icon"
                  title="Excluir Categoria"
                  @click="triggerDelete(cat)"
                >
                  🗑️
                </button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Categoria Modal -->
    <CategoriaModal
      :show="showFormModal"
      :categoria="selectedCategoria"
      @close="showFormModal = false"
      @save="handleSave"
    />

    <!-- Confirm Delete Dialog -->
    <ConfirmDialog
      :show="showDeleteDialog"
      title="Excluir Categoria"
      :message="`Tem certeza que deseja excluir a categoria '${categoryToDelete?.nome}'? Esta ação não poderá ser desfeita.`"
      @confirm="handleConfirmDelete"
      @cancel="showDeleteDialog = false"
    />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useCategorias } from '~/composables/useCategorias'
import CategoriaModal from '~/components/CategoriaModal.vue'
import ConfirmDialog from '~/components/ConfirmDialog.vue'

const {
  categorias,
  loading,
  fetchCategorias,
  createCategoria,
  updateCategoria,
  deleteCategoria
} = useCategorias()

const showFormModal = ref(false)
const showDeleteDialog = ref(false)
const selectedCategoria = ref(null)
const categoryToDelete = ref(null)

onMounted(() => {
  fetchCategorias()
})

const openCreateModal = () => {
  selectedCategoria.value = null
  showFormModal.value = true
}

const openEditModal = (cat) => {
  selectedCategoria.value = cat
  showFormModal.value = true
}

const handleSave = async (data) => {
  if (data.id) {
    // Update (PUT)
    return await updateCategoria(data.id, data.nome, data.descricao)
  } else {
    // Create (POST)
    return await createCategoria(data.nome, data.descricao)
  }
}

const triggerDelete = (cat) => {
  categoryToDelete.value = cat
  showDeleteDialog.value = true
}

const handleConfirmDelete = async () => {
  if (!categoryToDelete.value) return
  const success = await deleteCategoria(categoryToDelete.value.id)
  if (success) {
    showDeleteDialog.value = false
    categoryToDelete.value = null
  }
}
</script>
