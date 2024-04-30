<script>
export default {
  name: 'TreeItem', // necessary for self-reference
  props: {
    model: Object
  },
  data() {
    return {
      isOpen: false
    }
  },
  computed: {
    isFolder() {
      return this.model.children && this.model.children.length
    }
  },
  methods: {
    toggle() {
      if (this.isFolder) {
        this.isOpen = !this.isOpen
      }
    },
  }
}
</script>

<template>
    <li>
        <div :class="{ bold: isFolder }"
             @click="toggle">
            {{ model.name }}
            <span v-if="model.type == 'folder'"><v-icon icon="mdi-folder"></v-icon></span>
            <span v-else><v-icon icon="mdi-file"></v-icon></span>
            <span v-if="isFolder">[{{ isOpen ? '-' : '+' }}]</span>
        </div>
        <ul class="nostyle" v-show="isOpen" v-if="isFolder">
            <TreeItem
                      v-for="model in model.children"
                      :model="model">
            </TreeItem>
        </ul>
    </li>
</template>
<style>
    .mdi-folder {
        color:burlywood !important;
    }
    .mdi-file {
        color: cornflowerblue !important;
    }
</style>