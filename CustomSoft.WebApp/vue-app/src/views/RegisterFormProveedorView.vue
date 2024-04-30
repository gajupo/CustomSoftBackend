<template>
    <v-container>
        <h2>Registrar Proveedor</h2>
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
        data() {
            return {
                provider: {
                    nombre: '',
                    rfc: '',
                    direccion: '',
                    activo: false,
                },
            };
        },
        mounted() {
        },
        methods: {
            cancelAction() {
                this.$router.push({ name: 'home' });
            },
            async updateProvider() {
                console.log('creating provider:', this.provider);

                const result = await this.$api.post(`/proveedores`, this.provider);

                if (result) {
                    this.$router.push({ name: 'home', params: {} });
                }
            }
        }
    };
</script>
