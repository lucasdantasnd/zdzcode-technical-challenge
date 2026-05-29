<template>
  <div class="app-container">
    <!-- Sidebar Navigation -->
    <aside class="sidebar">
      <div class="brand">
        <div class="brand-icon">I</div>
        <span class="brand-name">Inventário</span>
      </div>

      <nav>
        <ul class="nav-links">
          <li>
            <NuxtLink to="/produtos" class="nav-link">
              <span class="nav-icon">📦</span>
              <span>Produtos</span>
            </NuxtLink>
          </li>
          <li>
            <NuxtLink to="/categorias" class="nav-link">
              <span class="nav-icon">📂</span>
              <span>Categorias</span>
            </NuxtLink>
          </li>
        </ul>
      </nav>
    </aside>

    <!-- Main Content Area -->
    <div class="main-wrapper">
      <header class="header">
        <div class="header-title">
          <h1>{{ currentTitle }}</h1>
          <p>{{ currentSubtitle }}</p>
        </div>
      </header>

      <main class="content-body">
        <NuxtPage />
      </main>
    </div>

    <!-- Global Toast Alerts -->
    <ToastNotification />
  </div>
</template>

<script setup>
import { computed } from 'vue'
import { useRoute } from 'vue-router'

const route = useRoute()

const currentTitle = computed(() => {
  if (route.path.includes('/categorias')) return 'Gestão de Categorias'
  if (route.path.includes('/produtos')) return 'Catálogo de Produtos'
  return 'Painel de Controle'
})

const currentSubtitle = computed(() => {
  if (route.path.includes('/categorias')) return 'Visualize, cadastre e gerencie a hierarquia do catálogo'
  if (route.path.includes('/produtos')) return 'Controle os produtos disponíveis, preços e associações'
  return 'Plataforma de Inventário Comercial'
})
</script>
