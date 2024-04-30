<template>
    <v-layout class="rounded rounded-md">
        <v-navigation-drawer>
            <ul class="nostyle">
                <TreeItem class="item" :model="treeData"></TreeItem>
            </ul>
        </v-navigation-drawer>

        <v-app-bar title="Agregar Facturas"></v-app-bar>

        <v-main class="d-flex flex-column mb-6">
            
            <v-form ref="form" @submit.prevent="uploadFiles">
                <v-text-field v-model="destinationFolder" label="Nombre de Carpeta (Optional)"></v-text-field>
                <v-sheet class="ma-2 pa-2">
                    <v-file-input v-model="files"
                                  label="Selecciona la facturas"
                                  multiple
                                  :show-size="true"
                                  prepend-icon="mdi-paperclip"></v-file-input>

                </v-sheet>
                <v-sheet class="ma-2 pa-2">
                    <v-btn :disabled="files.length === 0 || uploadingFiles"
                           color="primary"
                           class="mt-3"
                           type="submit">
                        Cargar
                    </v-btn>

                    <v-btn :disabled="uploadingFiles" @click="cancelAction"
                           color="primary"
                           class="mt-3 mx-1"
                           type="button">
                        Cancelar
                    </v-btn>
                    <v-progress-circular v-show="uploadingFiles" color="primary" class="mt-2"
                        indeterminate>
                    </v-progress-circular>
                </v-sheet>
            </v-form>
            
        </v-main>
    </v-layout>
</template>

<script>
import TreeItem from '../components/TreeItem.vue'

export default {
    components:{
        TreeItem
    },
    props: {
        id: Number
    },
  data() {
    return {
      files: [],
      provider: {},
      treeData: {},
      rawPaths: [],
      destinationFolder: '',
      uploadingFiles: false
    };
  },
  async mounted() {
     await this.loadProvider();
  },
    methods: {
        async loadProvider() {
            const result = await this.$api.get(`/proveedores/${this.id}`);

            if (result) {
                this.provider = result;
                this.rawPaths = result?.archivos.flatMap((file) => file.ruta)
                this.buildTree()
            }
        },
        cancelAction(){
            this.$router.push({ name: 'home' });
        },
        async uploadFiles() {
            if (this.files.length === 0) {
            return;
            }
            
            try{
                this.uploadingFiles = true
                const result = await this.$api.postFiles(`/proveedores/add-invoices`, this.files, { proveedorId: this.id, destinationFolder: this.destinationFolder });
                if(result){
                    this.files = []
                    this.loadProvider();
                    this.uploadingFiles = false
                }
            }
            catch
            {
                this.uploadingFiles = false
            }  
        },
        buildTree() {
            const rootNode = {
                name: "Facturas",
                type: "folder",
                children: []
            };

            this.rawPaths.forEach(path => {
                const parts = path.split('\\').slice(3);
                let currentNode = rootNode;
                parts.forEach((part, index) => {
                    let isFile = index === parts.length - 1 && part.includes('.');
                    let existingNode = currentNode.children.find(node => node.name === part);
                    if (!existingNode) {
                        existingNode = {
                            name: part,
                            type: isFile ? "file" : "folder",
                            children: []
                        };
                        currentNode.children.push(existingNode);
                    }
                    currentNode = existingNode;
                });
            });
            console.log(rootNode)
            this.treeData = rootNode
        }
    }
};
</script>
<style>
    .item {
        cursor: pointer;
        line-height: 1.5;
    }

    .bold {
        font-weight: bold;
    }
</style>