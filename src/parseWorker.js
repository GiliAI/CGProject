importScripts('../lib/zip/zip.js');
importScripts('../lib/zip/zip-fs.js');
importScripts('../lib/zip/zip-ext.js');

zip.workerScriptsPath = "./lib/zip/";

onmessage = function (event) {
    loadData();

};

function loadData() {
    var zipFs = new zip.fs.FS();

    function onerror(message) {
        console.error(message);
    }

    zipFs.importHttpContent('./asset/pack.zip', false, function () {
        console.log('unzip finish');
        let entries = zipFs.root.children;
        entries.forEach(function (item) {
            console.log(item);
            // parseWorker.postMessage(item);
            // new Promise(function (resolve,reject) {
            //     item.getText(function (data) {
            //         console.log(data);
            //         // console.log(str2ab(data));
            //     });
            //     resolve();
            // });
        });

        // entries.forEach(function (item) {
        //     // console.log(item.name);
        //     item.getText(function (data) {
        //         console.log(data);
        //     });
        //     // new Promise(function (resolve,reject) {
        //     //     resolve(item);
        //     // }).then(function (entry) {
        //     //     entry.getBlob('application/octet-stream', function (data) {
        //     //         let arrayBuffer;
        //     //         let name = entry.name;
        //     //         let fileReader = new FileReader();
        //     //         fileReader.onload = function (event) {
        //     //             arrayBuffer = event.target.result;
        //     //         };
        //     //         fileReader.onloadend = function (event) {
        //     //             console.log(name,arrayBuffer);
        //     //             // loader.parse(arrayBuffer, './', (gltf) => {
        //     //             //     let mesh = gltf.scene.children[0];
        //     //             //     mesh.scale.set(0.1, 0.1, 0.1);
        //     //             //     scene.add(mesh);
        //     //             // });
        //     //         };
        //     //         fileReader.readAsArrayBuffer(data);
        //     //     });
        //     // });
        // });

        // var firstEntry = zipFs.root.children[0];
        // console.log(firstEntry);
        // console.log(zipFs.root.children.length);
        // firstEntry.getBlob('application/octet-stream', function (data) {
        //
        // });
    }, onerror);
}