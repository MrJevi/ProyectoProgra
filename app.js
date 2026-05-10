const API_URL = 'https://localhost:7051/api/Libros';
let librosData = [];


async function cargarCatalogo() {
    try {
        const res = await fetch(API_URL);
        if (res.ok) {
            librosData = await res.json();
            document.getElementById('count-libros').innerText = librosData.length;
            dibujarLibros(librosData);
        }
    } catch (e) {
        console.error("Error: No se pudo conectar a la API local.");
    }
}


function dibujarLibros(lista) {
    const contenedor = document.getElementById('contenedor-libros');
    contenedor.innerHTML = '';

    lista.forEach(libro => {
        contenedor.innerHTML += `
            <div class="book-card">
                <div class="portada-preview">📜</div>
                <div class="book-content">
                    <span class="category-tag">${libro.categoria}</span>
                    <h3>${libro.titulo}</h3>
                    <p>${libro.autor}</p>
                    
                    <div class="btn-acciones">
                        <button class="btn-main" onclick="abrirPDF('${libro.pdfUrl}')">Leer PDF</button>
                        <button class="btn-sec" onclick="solicitarPrestamo('${libro.titulo}')">Préstamo</button>
                    </div>
                    <button class="btn-del" onclick="eliminarLibro(${libro.id})">Retirar de Estante</button>
                </div>
            </div>
        `;
    });
}


window.filtrar = function() {
    const query = document.getElementById('buscador').value.toLowerCase();
    const filtrados = librosData.filter(l => 
        l.titulo.toLowerCase().includes(query) || l.autor.toLowerCase().includes(query)
    );
    dibujarLibros(filtrados);
};


document.getElementById('formLibro').addEventListener('submit', async (e) => {

    e.preventDefault();

    const archivo = document.getElementById('archivo').files[0];

    // Guardar libro
    const libro = {
        id: Date.now(),
        titulo: document.getElementById('titulo').value,
        autor: document.getElementById('autor').value,
        categoria: document.getElementById('categoria').value
    };

    try {

        // POST libro
        await fetch("https://localhost:7157/api/libros", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(libro)
        });

        // POST PDF
        const formData = new FormData();
        formData.append('archivo', archivo);

        await fetch("https://localhost:7157/api/libros/subir-pdf", {
            method: 'POST',
            body: formData
        });

        Swal.fire(
            'Éxito',
            'Libro y PDF guardados correctamente',
            'success'
        );

        document.getElementById('formLibro').reset();

        cargarCatalogo();

    } catch (error) {

        Swal.fire(
            'Error',
            error.message,
            'error'
        );

        console.error(error);
    }
});


window.eliminarLibro = async function(id) {
    const confirmacion = await Swal.fire({
        title: '¿Desea eliminar libro?',
        text: "Esta acción lo eliminará permanentemente de la biblioteca.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#6b4226',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    });

    if (confirmacion.isConfirmed) {
        try {
            const res = await fetch(`${API_URL}/${id}`, { method: 'DELETE' });
            if (res.ok) {
                Swal.fire('Eliminado', 'El libro ha sido retirado.', 'success');
                cargarCatalogo();
            }
        } catch (e) {
            Swal.fire('Error', 'No se pudo eliminar.', 'error');
        }
    }
};


window.abrirPDF = function(url) {
    if (!url || url === "null") {
        Swal.fire('Aviso', 'Este ejemplar no tiene PDF disponible.', 'info');
        return;
    }
    
    window.open(`https://localhost:7051/Uploads/Libros/${url}`, '_blank');
};


window.solicitarPrestamo = function(titulo) {
    Swal.fire({
        title: 'Préstamo Solicitado',
        text: `Se ha registrado tu solicitud para "${titulo}".`,
        icon: 'success',
        confirmButtonColor: '#6b4226'
    });
};


window.onload = cargarCatalogo;

		
