<template>
    <v-container>
        <v-form @submit.prevent="updateProvider">
            <v-text-field v-model="provider.nombre"
                          label="Nombre del Proveedor"
                          required></v-text-field>

            <v-text-field v-model="provider.rfc"
                          label="RFC"
                          required></v-text-field>

            <v-text-field v-model="provider.direccion"
                          label="Dirección"
                          required></v-text-field>

            <v-switch v-model="provider.activo"
                      label="Activo"></v-switch>

            <v-btn color="primary" type="submit">
                Guardar
            </v-btn>
            <v-btn class="mx-2" @click="cancelAction" color="primary" type="submit">
                Cancelar
            </v-btn>
        </v-form>
    </v-container>
</template>

<script>
    export default {
        props: {
            id: Number
        },
        data() {
            return {
                provider: {
                    id: this.id,
                    nombre: '',
                    fechaAlta: '',
                    rfc: '',
                    direccion: '',
                    activo: false,
                    fechaCreacion: '',
                    archivos: []
                },
            };
        },
        async mounted() {
            await this.loadProvider();
        },
        methods: {
            cancelAction() {
                this.$router.push({ name: 'home' });
            },
            async loadProvider() {
                const result = await this.$api.get(`/proveedores/${this.provider.id}`);

                if (result) {
                    this.provider = result;
                }
            },
            async updateProvider() {
                // Here you would make an API call to update the provider
                console.log('Updating provider:', this.provider);

                const result = await this.$api.put(`/proveedores/${this.provider.id}`, this.provider);

                if (result) {
                    this.$router.push({ name: 'home', params: {} });
                }
            }
        }
    };
</script>
