async function recordMicrophone(dotnetReference) {
    await navigator.getUserMedia(
        {
            audio: true
        },
        function (audioStream) {
            let stream = new MediaStream(audioStream);

            let recorder = new MediaRecorder(stream);

            recorder.ondataavailable = e => {
                let cchunks = [];
                cchunks.push(e.data);
                const completeBlob = new Blob(cchunks);
                var reader = new window.FileReader();
                reader.readAsDataURL(completeBlob);
                reader.onloadend = function () {
                    dotnetReference.invokeMethodAsync("ScreenRecordedCallback", reader.result.split(',')[1]);
                }
            };

            recorder.start(50);

            document.getElementById("btnStop").addEventListener("click", () => {
                stream.getTracks().forEach(t => t.stop());
                dotnetReference.invokeMethodAsync("Finished");
            });

            stream.getAudioTracks()[0].addEventListener('ended', () => {
                stream.getTracks().forEach(t => t.stop());
                dotnetReference.invokeMethodAsync("Finished");
            });
        },
        function (e) {
            alert('Error capturing audio.');
        }
    );
}

function analyzeMicrophone(dotnetReference, language, token) {
    let result;
    var speechConfig = SpeechSDK.SpeechConfig.fromAuthorizationToken(token, "switzerlandnorth");
    speechConfig.speechRecognitionLanguage = language;
    var audioConfig = SpeechSDK.AudioConfig.fromDefaultMicrophoneInput();
    recognizer = new SpeechSDK.SpeechRecognizer(speechConfig, audioConfig);

    recognizer.recognizeOnceAsync(
        function (result) {
            result += result.text;
            dotnetReference.invokeMethodAsync("MicrophoneAnalyzedCallback", result, language);

            recognizer.close();
            recognizer = undefined;
        },
        function (err) {
            phraseDiv.innerHTML += err;
            window.console.log(err);

            recognizer.close();
            recognizer = undefined;
        });

    window.onload += () => {
        document.getElementById("btnStop").addEventListener("click", () => {
            console.log(90);
            recognizer.close();
        });
    }
}