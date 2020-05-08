onmessage = function (event) {
    new Promise(function (resolve) {
        let xhr = new XMLHttpRequest();
        xhr.open('GET', '../asset/data/hongyeTestModel/glbLength.json', true);
        xhr.onload = function (e) {
            resolve(JSON.parse(this.response));
        };
        xhr.send();
    }).then(function (array) {
        new Promise(function (resolve) {
            let xhr = new XMLHttpRequest();
            xhr.open('GET', '../asset/data/hongyeTestModel/pack.glb', true);
            xhr.responseType = 'arraybuffer';
            xhr.onload = function (e) {
                splitGlbArr(new Uint8Array(this.response), array);
                resolve();
            };
            xhr.send();
        });
    });
};

function splitGlbArr(glbArr, indArr) {
    let totalLength = 0;
    for (let i = 0; i < indArr.length; i++) {
        postMessage(glbArr.slice(totalLength, totalLength + indArr[i]));
        totalLength += indArr[i];
    }
}