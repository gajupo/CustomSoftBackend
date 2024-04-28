<template>
    <v-layout class="rounded rounded-md">
        <v-app-bar title="Application bar"></v-app-bar>

        <v-navigation-drawer>
            <v-list>
                <v-list-item title="Navigation drawer"></v-list-item>
            </v-list>
        </v-navigation-drawer>

        <v-main class="d-flex align-center justify-center" style="min-height: 300px;">
            <h1>Provider List</h1>
            <p>Show the list of registered providers</p>

            <div v-if="loading" class="loading">
                Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
            </div>

            <div v-if="post">
                <table>
                    <thead>
                        <tr>
                            <th>Nombre</th>
                            <th>RFC</th>
                            <th>Direccion</th>
                            <th>Fecha Alta</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="proveedor in post" :key="proveedor.Id">
                            <td>{{ proveedor.nombre }}</td>
                            <td>{{ proveedor.rfc }}</td>
                            <td>{{ proveedor.direccion }}</td>
                            <td>{{ proveedor.fechaAlta }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </v-main>
    </v-layout>
</template>
<script lang="js">
    import { defineComponent } from 'vue';

    const proveedoredEnddPoint = 'https://localhost:7135';

    export default defineComponent({
        data() {
            return {
                loading: false,
                post: null
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            fetchData() {
                this.post = null;
                this.loading = true;

                fetch(`${proveedoredEnddPoint}/proveedores`)
                    .then(r => r.json())
                    .then(json => {
                        this.post = json;
                        this.loading = false;
                        return;
                    });
            }
        },
    });
</script>